using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    internal partial class ConfigListPanel<T> : VisualElement where T : ScriptableObject
    {
        protected const int RowHeight = 30;
        protected const int Indent = 16;
        protected const string DndId = "ConfigListPanel_DragConfig";

        protected readonly ListView _listView;
        protected readonly List<T> _allConfigs = new();
        protected readonly List<Row> _flat = new();
        private readonly HashSet<string> _expandedFolders = new();
        private readonly HashSet<string> _collapsedFilteredFolders = new();
        private FolderNode _rootFolder;

        protected string _rootFolderPath;
        protected string _searchText = "";
        protected HashSet<string> _selectedTags = new();

        public Func<string, T, bool> CustomFilter;

        protected enum EditMode { None, Config, Folder }
        protected EditMode _editMode = EditMode.None;
        protected T _editingConfig;
        protected TextField _editingField;
        protected string _editingFolderPath;
        protected TextField _editingFolderField;

        private class FolderNode
        {
            public string Path;
            public List<T> Items;
            public List<FolderNode> Children;
        }

        private bool IsFiltering => !string.IsNullOrWhiteSpace(_searchText) || _selectedTags.Count > 0;

        public event Action<T> OnConfigSelected;
        public event Action<string> OnFolderSelected;
        public event Action<string> OnFolderDeleteRequested;
        public event Action<T> OnDuplicateConfigRequested;
        public event Action<T> OnConfigDeleteRequested;
        public event Action<string> OnCreateConfigRequested;
        public event Action<string> OnCreateFolderRequested;
        public event Action OnUpdateRequested;
        public event Action<T, string> OnConfigRenameRequested;

        public ConfigListPanel()
        {
            style.flexGrow = 1;
            style.flexShrink = 1;
            style.minHeight = 0;

            _listView = new ListView
            {
                selectionType = SelectionType.Single,
                showBorder = true,
                virtualizationMethod = CollectionVirtualizationMethod.FixedHeight,
                fixedItemHeight = RowHeight
            };

            _listView.AddManipulator(new ContextualMenuManipulator(OnContextClick));

            _listView.style.flexGrow = 1;
            _listView.style.flexShrink = 1;
            _listView.style.minHeight = 0;

            _listView.makeItem = MakeRow;
            _listView.bindItem = BindRow;

            // чтобы даже при пустом источнике у нас была правильная верстка
            _listView.itemsSource = _flat;

            Add(_listView);
        }

        private void OnContextClick(ContextualMenuPopulateEvent evt)
        {
            string folderPath = null;
            for (var element = evt.target as VisualElement; element != null && element != _listView; element = element.parent)
            {
                if (element.userData is RowWidgets { Row: { } row })
                {
                    folderPath = row.FolderPath;
                    break;
                }
            }
            evt.menu.AppendAction("New config", _ => OnCreateConfigRequested?.Invoke(folderPath), DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendAction("New folder", _ => OnCreateFolderRequested?.Invoke(folderPath), DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("Update", _ => OnUpdateRequested?.Invoke(), DropdownMenuAction.AlwaysEnabled);
        }

        public void BindTo(IEnumerable<T> configs, string folderPath)
        {
            string normalizedPath = folderPath?.Replace('\\', '/').TrimEnd('/');
            if (_rootFolderPath != normalizedPath)
            {
                _expandedFolders.Clear();
                _rootFolderPath = normalizedPath;
            }

            _allConfigs.Clear();
            if (configs != null)
                _allConfigs.AddRange(configs.Where(config => config != null));
            Filter(_searchText, _selectedTags);
        }

        public void Filter(string searchText, HashSet<string> selectedTags = null)
        {
            _searchText = searchText ?? "";
            _selectedTags = selectedTags ?? new HashSet<string>();

            IEnumerable<T> query;
            if (CustomFilter == null)
            {
                query = _allConfigs;
                if (!string.IsNullOrWhiteSpace(_searchText))
                    query = query.Where(c => !string.IsNullOrEmpty(c.name) &&
                                             c.name.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else
            {
                query = _allConfigs.Where(c => CustomFilter(searchText, c));
            }
            
            if (_selectedTags.Count > 0)
            {
                query = query.Where(c =>
                {
                    return c.GetTagsFromDatabase().Any(tag => _selectedTags.Contains(tag));
                });
            }

            _collapsedFilteredFolders.Clear();
            _rootFolder = null;
            if (!string.IsNullOrEmpty(_rootFolderPath) && AssetDatabase.IsValidFolder(_rootFolderPath))
            {
                var itemsByFolder = query.ToLookup(GetConfigFolder);
                _rootFolder = BuildFolder(_rootFolderPath, itemsByFolder);
            }
            RefreshRows();
        }

        public void ScrollToAndEditConfig(T config)
        {
            if (config == null) return;

            string folder = GetConfigFolder(config);
            if (string.IsNullOrEmpty(folder)) return;

            RevealFolder(folder);

            int idx = _flat.FindIndex(r => !r.IsFolder && Equals(r.Config, config));
            if (idx >= 0)
            {
                _listView.selectedIndex = idx;
                _listView.ScrollToItem(idx);
                StartConfigInlineEdit(config);
            }
        }

        public void ScrollToFolder(string folderPath)
        {
            RevealFolder(folderPath);
            int index = _flat.FindIndex(row => row.IsFolder && row.FolderPath == folderPath);
            if (index >= 0)
                _listView.ScrollToItem(index);
        }

        private static string GetConfigFolder(T config)
        {
            return GetParentFolder(AssetDatabase.GetAssetPath(config));
        }

        private static string GetParentFolder(string path)
        {
            return Path.GetDirectoryName(path)?.Replace('\\', '/');
        }

        private FolderNode BuildFolder(string path, ILookup<string, T> itemsByFolder)
        {
            var children = AssetDatabase.GetSubFolders(path)
                .OrderBy(childPath => childPath, StringComparer.OrdinalIgnoreCase)
                .Select(childPath => BuildFolder(childPath, itemsByFolder))
                .Where(child => child != null)
                .ToList();

            var items = itemsByFolder[path].OrderBy(config => config.name, StringComparer.OrdinalIgnoreCase).ToList();
            if (IsFiltering && children.Count == 0 && items.Count == 0)
                return null;

            return new FolderNode { Path = path, Items = items, Children = children };
        }

        private void RefreshRows()
        {
            var selected = _listView.selectedItem as Row;
            _flat.Clear();
            if (_rootFolder != null)
                AppendFolderContents(_rootFolder, 0);
            _listView.Rebuild();

            if (selected != null)
            {
                int index = _flat.FindIndex(row => row.IsFolder == selected.IsFolder &&
                    (row.IsFolder ? row.FolderPath == selected.FolderPath : row.Config == selected.Config));
                _listView.SetSelectionWithoutNotify(index >= 0 ? new[] { index } : Array.Empty<int>());
            }
        }

        private void AppendFolderContents(FolderNode folder, int depth)
        {
            foreach (var child in folder.Children)
            {
                bool canExpand = child.Children.Count > 0 || child.Items.Count > 0;
                bool expanded = canExpand && (IsFiltering
                    ? !_collapsedFilteredFolders.Contains(child.Path)
                    : _expandedFolders.Contains(child.Path));
                _flat.Add(new Row
                {
                    IsFolder = true,
                    FolderPath = child.Path,
                    FolderName = Path.GetFileName(child.Path),
                    CanExpand = canExpand,
                    Expanded = expanded,
                    Depth = depth
                });
                if (expanded)
                    AppendFolderContents(child, depth + 1);
            }

            foreach (var config in folder.Items)
            {
                _flat.Add(new Row
                {
                    Config = config,
                    ConfigName = config.name,
                    FolderPath = folder.Path,
                    Depth = depth
                });
            }
        }

        private void ToggleFolder(string folderPath)
        {
            var toggledFolders = IsFiltering ? _collapsedFilteredFolders : _expandedFolders;
            if (!toggledFolders.Remove(folderPath))
                toggledFolders.Add(folderPath);

            RefreshRows();
        }

        private void RevealFolder(string folderPath)
        {
            bool changed = false;
            for (string path = folderPath; !string.IsNullOrEmpty(path) && path != _rootFolderPath; path = GetParentFolder(path))
                changed |= IsFiltering ? _collapsedFilteredFolders.Remove(path) : _expandedFolders.Add(path);
            if (changed)
                RefreshRows();
        }
    }
}
