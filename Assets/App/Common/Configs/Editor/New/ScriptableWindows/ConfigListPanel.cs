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
        protected List<T> _filteredConfigs = new();
        protected readonly List<Row> _flat = new();
        protected readonly Dictionary<string, FolderNode> _folders = new();
        protected readonly HashSet<string> _expanded = new();
        protected readonly List<T> _rootItems = new();

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

        protected class FolderNode
        {
            public string Path;
            public string Name;
            public readonly List<T> Items = new();
        }

        public event Action<T> OnConfigSelected;
        public event Action<string> OnFolderSelected;
        public event Action<string> OnFolderDeleteRequested;
        public event Action<T> OnDuplicateConfigRequested;
        public event Action<T> OnConfigDeleteRequested;
        public event Action<string> OnCreateConfigRequested;
        public event Action OnCreateFolderRequested;
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
            if (evt.target is VisualElement targetElement)
            {
                if (targetElement.userData is RowWidgets rowWidgets)
                {
                    var row = rowWidgets.Row;
                    if (row.IsFolder) folderPath = row.FolderPath;
                    else
                    {
                        var assetPath = AssetDatabase.GetAssetPath(row.Config);
                        var directory = Path.GetDirectoryName(assetPath);
                        if (directory != _rootFolderPath) folderPath = directory;
                    }
                }
            }
            evt.menu.AppendAction("New config", _ => OnCreateConfigRequested?.Invoke(folderPath), DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendAction("New folder", _ => OnCreateFolderRequested?.Invoke(), DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("Update", _ => OnUpdateRequested?.Invoke(), DropdownMenuAction.AlwaysEnabled);
        }

        public void BindTo(IEnumerable<T> configs, string folderPath)
        {
            _allConfigs.Clear();

            if (configs != null)
                _allConfigs.AddRange(configs.Where(x => x != null));
            foreach(var cfg in _rootItems.Where(c => c != null).OrderBy(c => c.name, StringComparer.OrdinalIgnoreCase))
            {
                if(!_allConfigs.Contains(cfg))
                    _allConfigs.Add(cfg);
            }

            // Если ничего не пришло — грузим сами все ассеты типа T
            if (_allConfigs.Count == 0)
                LoadAllAssetsIfNeeded();

            // Определяем корень (если не передан) по общему префиксу путей ассетов
            _rootFolderPath = string.IsNullOrEmpty(folderPath) ? FindCommonRoot(_allConfigs) : folderPath?.Replace('\\', '/');

            // Разворачиваем корневые папки по умолчанию
            _expanded.Clear();
            if (!string.IsNullOrEmpty(_rootFolderPath))
                _expanded.Add(_rootFolderPath);

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

            _filteredConfigs = query.ToList();

            RebuildFolders();
            RebuildFlat();

            _listView.itemsSource = _flat;
            _listView.Rebuild();
        }

        public void ScrollToAndEditConfig(T config)
        {
            if (config == null) return;

            string path = AssetDatabase.GetAssetPath(config);
            string folder = Path.GetDirectoryName(path)?.Replace('\\', '/');
            if (string.IsNullOrEmpty(folder)) return;

            _expanded.Add(folder);

            RebuildFlat();
            _listView.itemsSource = _flat;
            _listView.Rebuild();

            int idx = _flat.FindIndex(r => !r.IsFolder && Equals(r.Config, config));
            if (idx >= 0)
            {
                _listView.selectedIndex = idx;
                _listView.ScrollToItem(idx);
                StartConfigInlineEdit(config);
            }
        }

        private void LoadAllAssetsIfNeeded()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var cfg = AssetDatabase.LoadAssetAtPath<T>(path);
                if (cfg != null) _allConfigs.Add(cfg);
            }
        }

        private static string FindCommonRoot(List<T> assets)
        {
            if (assets == null || assets.Count == 0) return null;

            var allPaths = assets
                .Select(a => AssetDatabase.GetAssetPath(a))
                .Where(p => !string.IsNullOrEmpty(p))
                .Select(p => Path.GetDirectoryName(p)?.Replace('\\', '/'))
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .ToList();

            if (allPaths.Count == 0) return null;
            if (allPaths.Count == 1) return allPaths[0];

            // Находим общий префикс по сегментам
            var split = allPaths.Select(p => p.Split('/')).ToList();
            int minLen = split.Min(s => s.Length);
            int i = 0;
            for (; i < minLen; i++)
            {
                var seg = split[0][i];
                if (split.Any(s => s[i] != seg))
                    break;
            }
            if (i == 0) return null;
            return string.Join("/", split[0].Take(i));
        }

        private void RebuildFolders()
        {
            _folders.Clear();
            _rootItems.Clear();

            var root = _rootFolderPath;

            if (!string.IsNullOrEmpty(root) && AssetDatabase.IsValidFolder(root))
            {
                var source = _filteredConfigs ?? Enumerable.Empty<T>();
                foreach (var config in source)
                {
                    var p = AssetDatabase.GetAssetPath(config);
                    if (string.IsNullOrEmpty(p)) continue;
                    var cfgDir = Path.GetDirectoryName(p)?.Replace('\\', '/');
                    if (string.IsNullOrEmpty(cfgDir)) continue;

                    if (string.Equals(cfgDir, root, StringComparison.Ordinal))
                    {
                        _rootItems.Add(config);
                        continue;
                    }

                    var immediate = GetImmediateChildFolder(root, cfgDir);
                    var key = string.IsNullOrEmpty(immediate) ? cfgDir : immediate;

                    if (!_folders.TryGetValue(key, out var node) && key != root)
                    {
                        node = new FolderNode { Path = key, Name = Path.GetFileName(key) };
                        _folders[key] = node;
                    }

                    node?.Items.Add(config);
                }
            }
            else
            {
                var source = _filteredConfigs ?? Enumerable.Empty<T>();
                foreach (var config in source)
                {
                    var p = AssetDatabase.GetAssetPath(config);
                    if (string.IsNullOrEmpty(p)) continue;
                    var folderPath = Path.GetDirectoryName(p)?.Replace('\\', '/');
                    if (string.IsNullOrEmpty(folderPath)) { _rootItems.Add(config); continue; }

                    if (!_folders.TryGetValue(folderPath, out var node))
                    {
                        node = new FolderNode { Path = folderPath, Name = Path.GetFileName(folderPath) };
                        _folders[folderPath] = node;
                    }

                    node.Items.Add(config);
                }
            }
            // удаляем папки без элементов, чтобы не показывать их в списке
            var emptyKeys = _folders
                .Where(kvp => kvp.Value == null || kvp.Value.Items == null || kvp.Value.Items.Count == 0)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in emptyKeys)
                _folders.Remove(key);

            // раскрываем все оставшиеся папки при первой сборке
            foreach (var key in _folders.Keys)
                _expanded.Add(key);
        }

        private void RebuildFlat()
        {
            _flat.Clear();

            foreach (var cfg in _rootItems.Where(c => c != null).OrderBy(c => c.name, StringComparer.OrdinalIgnoreCase))
            {
                _flat.Add(new Row
                {
                    IsFolder = false,
                    Config = cfg,
                    ConfigName = cfg.name,
                    FolderPath = _rootFolderPath,
                    Depth = 0
                });
            }

            foreach (var folder in _folders.Values.OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase))
            {
                bool isExpanded = _expanded.Contains(folder.Path);

                if (folder.Path != _rootFolderPath?.Replace('\\', '/'))
                {
                    _flat.Add(new Row
                    {
                        IsFolder = true,
                        FolderPath = folder.Path,
                        FolderName = folder.Name,
                        Expanded = isExpanded,
                        Depth = 0
                    });
                }

                if (!isExpanded) continue;

                foreach (var cfg in folder.Items.OrderBy(c => c.name, StringComparer.OrdinalIgnoreCase))
                {
                    _flat.Add(new Row
                    {
                        IsFolder = false,
                        Config = cfg,
                        ConfigName = cfg.name,
                        FolderPath = folder.Path,
                        Depth = 1
                    });
                }
            }
        }
        
        protected void ToggleFolder(string folderPath)
        {
            if (_expanded.Contains(folderPath)) 
                _expanded.Remove(folderPath);
            else 
                _expanded.Add(folderPath);

            RebuildFlat();
            _listView.itemsSource = _flat;
            _listView.Rebuild();
        }

        protected static string GetImmediateChildFolder(string root, string targetDir)
        {
            if (string.IsNullOrEmpty(root) || string.IsNullOrEmpty(targetDir)) 
                return string.Empty;
                
            if (!targetDir.StartsWith(root, StringComparison.Ordinal)) 
                return string.Empty;

            var rest = targetDir.Length == root.Length ? string.Empty : targetDir.Substring(root.Length).TrimStart('/');
            if (string.IsNullOrEmpty(rest)) 
                return string.Empty;

            int slash = rest.IndexOf('/');
            string first = slash >= 0 ? rest.Substring(0, slash) : rest;
            return string.IsNullOrEmpty(first) ? string.Empty : $"{root}/{first}";
        }
    }
}