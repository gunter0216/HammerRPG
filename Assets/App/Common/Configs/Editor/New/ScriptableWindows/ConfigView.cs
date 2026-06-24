using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Configs.External;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    internal class ConfigView : VisualElement
    {
        private readonly ConfigWindow _window;
        private readonly PropertiesPanel<GameConfig> _propertiesPanel;

        private SearchAndFilterPanel<GameConfig> _searchPanel;
        private ConfigListPanel<GameConfig> _configListPanel;
        
        public ConfigView(ConfigWindow window, VisualElement propertiesView)
        {
            AddToClassList("config-view");
 
            _window = window;
            _searchPanel = new SearchAndFilterPanel<GameConfig>();
            _propertiesPanel = new PropertiesPanel<GameConfig>(propertiesView);

            _propertiesPanel.SetAddButtonVisible(_window.Selected != null);
            _propertiesPanel.OnTagAdded += tag =>
            {
                var config = _window.Selected;
                if (config == null || string.IsNullOrWhiteSpace(tag)) return;

                var tags = GetTagsListRef(config);
                if (!tags.Contains(tag))
                    tags.Add(tag);

                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();

                _searchPanel.RefreshAvailableTags(_window.Collection);
                _propertiesPanel.RefreshTags();
            };
            _propertiesPanel.OnTagsChanged += newTags =>
            {
                var cfg = _window.Selected;
                if (cfg == null) return;

                var tags = GetTagsListRef(cfg);
                tags.Clear();
                foreach (var t in newTags)
                {
                    var v = (t ?? "").Trim();
                    if (!string.IsNullOrEmpty(v) && !tags.Contains(v))
                        tags.Add(v);
                }

                EditorUtility.SetDirty(cfg);
                AssetDatabase.SaveAssets();

                _searchPanel.RefreshAvailableTags(_window.Collection);
                _propertiesPanel.RefreshTags();
            };
            _propertiesPanel.OnDeleteTagRequested += tag =>
            {
                if (string.IsNullOrWhiteSpace(tag)) return;

                bool confirm = EditorUtility.DisplayDialog(
                    "Удалить тег",
                    $"Действительно ли вы хотите навсегда удалить тег \"{tag}\"?",
                    "Удалить", "Отмена");

                if (!confirm) return;

                bool anyChanged = false;
                foreach (var cfg in _window.Collection)
                {
                    var tags = GetTagsListRef(cfg);
                    if (tags.RemoveAll(t => string.Equals(t, tag, StringComparison.Ordinal)) > 0)
                    {
                        EditorUtility.SetDirty(cfg);
                        anyChanged = true;
                    }
                }

                if (anyChanged)
                    AssetDatabase.SaveAssets();

                _searchPanel.RefreshAvailableTags(_window.Collection);

                if (_window.Selected != null)
                    _propertiesPanel.BindSelectedTags(GetTagsListRef(_window.Selected), () => _searchPanel.GetAvailableTags());

                _propertiesPanel.RefreshTags();
            };

            if (_window.Selected != null)
            {
                _propertiesPanel.BindSelectedTags(GetTagsListRef(_window.Selected), () => _searchPanel.GetAvailableTags());
            }

            _window.OnConfigsChanged += () =>
            {
                _searchPanel.RefreshAvailableTags(_window.Collection);
                _configListPanel.BindTo(_window.Collection, _window.FolderPath);

                _propertiesPanel.SetAddButtonVisible(_window.Selected != null);
            };
            
            _propertiesPanel.OnTagAdded += tag =>
            {
                var config = _window.Selected;
                if (config == null || string.IsNullOrWhiteSpace(tag)) return;

                var db = ScriptableObjectTagsDatabase.Instance;
                if (db == null) return;

                db.AddTag(config, tag);

                _searchPanel.RefreshAvailableTags(_window.Collection);
                _propertiesPanel.RefreshTags();
            };
            
            _propertiesPanel.OnTagAdded += tag =>
            {
                var config = _window.Selected;
                if (config == null || string.IsNullOrWhiteSpace(tag)) return;

                var db = ScriptableObjectTagsDatabase.Instance;
                if (db == null) return;

                db.AddTag(config, tag);

                _searchPanel.RefreshAvailableTags(_window.Collection);
                _propertiesPanel.RefreshTags();
            };
            
            _propertiesPanel.OnTagAdded += tag =>
            {
                var config = _window.Selected;
                if (config == null || string.IsNullOrWhiteSpace(tag)) return;

                var db = ScriptableObjectTagsDatabase.Instance;
                if (db == null) return;

                db.AddTag(config, tag);

                _searchPanel.RefreshAvailableTags(_window.Collection);
                _propertiesPanel.RefreshTags();
            };

            SetupLayout();
            BindData();
        }

        private void SetupLayout()
        {
            var container = new TwoPaneSplitView(0, 400, TwoPaneSplitViewOrientation.Horizontal);
            var leftColumn = new VisualElement
            {
                name = "LeftColumn",
                style = { flexShrink = 1, marginRight = 10 }
            };
            leftColumn.Add(_searchPanel = new SearchAndFilterPanel<GameConfig>());
            leftColumn.Add(_configListPanel = new ConfigListPanel<GameConfig>());

            _configListPanel.CustomFilter = _window.CustomFilter;
            
            var rightColumn = new VisualElement
            {
                name = "RightColumn",
                style = { flexGrow= 1 }
            };
            rightColumn.Add(_propertiesPanel);

            container.Add(leftColumn);
            container.Add(rightColumn);

            Add(container);
            
            _searchPanel.OnSearchChanged += s =>  _configListPanel.Filter(s, _searchPanel.GetSelectedTags());
            _searchPanel.OnTagsChanged += s =>  _configListPanel.Filter(_searchPanel.CurrentSearchText, s);

            _configListPanel.OnCreateFolderRequested += CreateFolder;
            _configListPanel.OnCreateConfigRequested += CreateConfig;
            _configListPanel.OnDuplicateConfigRequested += DuplicateConfig;
            _configListPanel.OnConfigDeleteRequested += DeleteConfig;
            _configListPanel.OnUpdateRequested += UpdatesList;
            
            _window.OnConfigsChanged += () =>
            {
                _searchPanel.RefreshAvailableTags(_window.Collection);
                _configListPanel.BindTo(_window.Collection, _window.FolderPath);

                _propertiesPanel.SetAddButtonVisible(_window.Selected != null);
            };

            _window.OnFolderChanged += createdPath =>
            {
                _window.Select(null);
                _window.SelectFolder(createdPath);

                //_configListPanel.StartFolderInlineRename(createdPath);
            };

            _window.OnFolderSelected += createdPath =>
            {
                _window.Select(null);
                _window.SelectFolder(createdPath);

                //_configListPanel.StartFolderInlineRename(createdPath);
            };

            _searchPanel.RefreshAvailableTags(_window.Collection);
            _configListPanel.OnConfigRenameRequested += RenameConfig;
            _configListPanel.OnConfigSelected += SelectConfig;
            _configListPanel.OnFolderSelected += SelectFolder;
            _configListPanel.OnFolderDeleteRequested += path =>
            {
                _window.DeleteFolder(path);
                BindData();
                _propertiesPanel.SetAddButtonVisible(_window.Selected != null);
            };
        }

        private void DuplicateConfig(GameConfig config)
        {
            _window.DuplicateConfig(config);
        }

        public void RenameConfig(GameConfig config, string newName)
        {
            _window.RenameConfig(config, newName);
        }

        private void SelectConfig(GameConfig config)
        {
            _window.Select(config);
            var tags = config != null ? GetTagsListRef(config) : new List<string>();
            _propertiesPanel.BindSelectedTags(tags, () => _searchPanel.GetAvailableTags());
            _propertiesPanel.SetAddButtonVisible(config != null);
        }

        private void SelectFolder(string folderPath)
        {
            _window.SelectFolder(folderPath);
            _window.Select(null);
            
            _propertiesPanel.SetAddButtonVisible(false);
        }

        public void CreateConfig(string folder)
        {
            _window.CreateConfig(GetNewConfigName(), folder);
            
            BindData();

            _propertiesPanel.SetAddButtonVisible(_window.Selected != null);
            _configListPanel.ScrollToAndEditConfig(_window.Selected);
        }

        public void CreateFolder()
        {
            _window.CreateNewFolder();
            
            BindData();

            _propertiesPanel.SetAddButtonVisible(_window.Selected != null);
            _configListPanel.ScrollToAndEditConfig(_window.Selected);
        }

        public void UpdatesList()
        {
            BindData();

            _propertiesPanel.SetAddButtonVisible(_window.Selected != null);
            _configListPanel.ScrollToAndEditConfig(_window.Selected);
        }

        private static List<string> GetTagsListRef(GameConfig cfg)
        {
#if UNITY_EDITOR
            if (cfg == null)
                return new List<string>();

            var db = ScriptableObjectTagsDatabase.Instance;
            if (db == null)
                return new List<string>();

            return db.GetTags(cfg, createIfMissing: true);
#else
    return new List<string>();
#endif
        }
        
        private string GetNewConfigName()
        {
            const string baseName = "Config";
            var counter = 1;
            string newName;

            do
            {
                newName = $"{baseName}_{counter}";
                counter++;
            }
            while (_window.Collection.Any(c => c != null && !string.IsNullOrEmpty(c.name) && c.name == newName));

            return newName;
        }
        
        private void DeleteConfig(GameConfig config)
        {
            if (config is not null)
            {
                _window.DeleteConfig(config);
            }
            
            BindData();

            _propertiesPanel.SetAddButtonVisible(_window.Selected != null);
        }

        private void BindData()
        {
            _configListPanel.BindTo(_window.Collection, _window.FolderPath);
        }
    }
}