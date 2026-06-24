using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Common.Configs.Editor;
using App.Common.Configs.External;
using Game.Core.Utils.Common;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    public sealed class ConfigWindow : EditorWindow
    {
        public event Action OnConfigsChanged;
        public event Action<string> OnFolderChanged;
        public event Action<GameConfig> OnConfigSelected;
        public event Action<string> OnFolderSelected;

        public string FolderPath => Path.Combine("Assets", "_Content", "Data", Title);
        
        public GameConfig Selected => _selected;
        
        public IReadOnlyCollection<GameConfig> Collection => _collection;

        private GameConfig _selected;
        private string _selectedFolder;
        protected readonly List<Type> _compositionTypes = new();

        protected readonly List<GameConfig> _collection = new();
        
        private SerializedObject _serializedObject;
        private ConfigView _view;
        private VisualElement _propertiesContainer;
        private InspectorElement _inspector;
        
        private static string Title => "Game Config Window";
        
        public Func<string, GameConfig, bool> CustomFilter => null;

        public void Select(GameConfig config)
        {
            _selected = config;
            
            if (_selected == null)
            {
                _propertiesContainer.visible = false;
                return;
            }

            _serializedObject = new SerializedObject(config);
            _propertiesContainer.visible = true;
            
            Bind(_serializedObject);

            OnConfigSelected?.Invoke(config);
        }
        public void SelectFolder(string folderPath)
        {
            _selectedFolder = folderPath;
            _propertiesContainer.visible = false;

            //OnFolderSelected?.Invoke(folderPath);
        }

        public void UpdateList()
        {
            OnConfigSelected?.Invoke(null);
            OnFolderSelected?.Invoke(string.Empty);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Config Editor")]
        public static void OpenWindow()
        {
            GetWindow<ConfigWindow>(false, Title);
        }

        protected void CreateProperties(VisualElement root)
        {
            _inspector = root.Append(new InspectorElement());
        }

        protected void Bind(SerializedObject serializedObject)
        {
            if (_inspector == null)
            {
                Debug.LogError("Inspector is null");
                return;
            }
            
            _inspector.Bind(serializedObject);
        }

        protected void OnEnable()
        {
            if (_view != null) 
                return;
            
            DrawGraph();
            LoadCollection();
        }
        
        private void OnFocus() => OnEnable();

        private void DrawGraph()
        {
            _propertiesContainer = new VisualElement
            {
                visible = false,
            };

            var styleSheet = (StyleSheet)EditorGUIUtility.Load("Configs/ConfigWindow.uss");

            _view = new ConfigView(this, _propertiesContainer);
            _view.styleSheets.Add(styleSheet);
            _view.StretchToParentSize();
            
            CreateProperties(_propertiesContainer);

            rootVisualElement.Add(_view);
        }

        private void LoadCollection()
        {
            string[] assetPaths = Directory.GetFiles(
                ConfigEditorWindow.ConfigFolder, "*.asset",
                SearchOption.AllDirectories);

            foreach (string path in assetPaths)
            {
                var config = AssetDatabase.LoadAssetAtPath<GameConfig>(path.Replace("\\", "/"));
                if (config != null)
                    _collection.Add(config);
            }
            
            OnConfigsChanged?.Invoke();
        }

        internal void RenameConfig(GameConfig config, string newName)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings not found. Init Addressables first.");
                return;
            }
            
            var entry = settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(config)));
            entry.SetAddress($"{Title}/{newName}");

            config.name = newName;
            
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(config), newName);
            AssetDatabase.SaveAssets();
            
            OnRenamed();
        }

        internal void CreateConfig(string assetName) => CreateConfig(assetName, _selectedFolder);

        internal void CreateConfig(string assetName, string folderPath)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings not found. Init Addressables first.");
                return;
            }
            
            const string groupName = "Config";
            
            var group = settings.FindGroup(groupName);
            if (group == null)
            {
                group = settings.CreateGroup(groupName, false, false, false, null, typeof(BundledAssetGroupSchema));
            }

            EnsureFolders(FolderPath.Split(Path.DirectorySeparatorChar));

            if(string.IsNullOrEmpty(folderPath))
            {
                folderPath = FolderPath;
            }

            var path = Path.Combine(folderPath, $"{assetName}.asset");
            if (File.Exists(path))
            {
                var assetNameArray = assetName.Split('_');
                if (assetName.Contains('_') && int.TryParse(assetNameArray.Last(), out int number))
                {
                    assetName = $"{assetNameArray.First()}_{number + 1}";
                }
                else
                {
                    assetName = $"{assetName}_1";
                }
                path = Path.Combine(folderPath, $"{assetName}.asset");
            }
            var config = CreateInstance<GameConfig>();

            AssetDatabase.CreateAsset(config, path);

            var guid = AssetDatabase.AssetPathToGUID(path);
            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.SetAddress($"{Title}/{config.name}");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Created and added {path} to Addressables group {group.Name}");

            UpdateList();

            _collection.Add(config);

            OnConfigsChanged?.Invoke();
            
            OnCreated();
            Select(config);
        }
        
        internal void DuplicateConfig(GameConfig config)
        {
            var path = AssetDatabase.GetAssetPath(config);
            var newPath = AssetDatabase.GenerateUniqueAssetPath($"{Path.GetDirectoryName(path)}/{config.name}_copy.asset");
            AssetDatabase.CopyAsset(path, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            var newConfig = AssetDatabase.LoadAssetAtPath<GameConfig>(newPath);
            _collection.Add(newConfig);
            OnConfigsChanged?.Invoke();

            OnDuplicated();
            Select(newConfig);
        }
        

        internal void DeleteConfig(GameConfig config)
        {
            var path = AssetDatabase.GetAssetPath(config);
            var guid = AssetDatabase.AssetPathToGUID(path);

            if (_selected == config)
            {
                Select(null);
            }
            
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings != null)
            {
                settings.RemoveAssetEntry(guid);
            }
            
            _collection.Remove(config);

            OnConfigsChanged?.Invoke();
            AssetDatabase.DeleteAsset(path);
        }
        internal void CreateNewFolder()
        {
            EnsureFolders(FolderPath.Split(Path.DirectorySeparatorChar));

            string targetParent = FolderPath;

            const string leaf = "New Folder";
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(
                Path.Combine(targetParent, leaf).Replace('\\', '/'));

            string parent = Path.GetDirectoryName(uniquePath).Replace('\\', '/');
            string name = Path.GetFileName(uniquePath);

            AssetDatabase.CreateFolder(parent, name);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            string createdPath = Path.Combine(parent, name).Replace('\\', '/');
            OnFolderChanged?.Invoke(createdPath);
        }

        internal void DeleteFolder(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !AssetDatabase.IsValidFolder(folderPath))
                return;

            string normRoot = FolderPath.Replace('\\', '/').TrimEnd('/');
            string normFolder = folderPath.Replace('\\', '/').TrimEnd('/');

            if (!(normFolder + "/").StartsWith(normRoot + "/", StringComparison.Ordinal))
            {
                EditorUtility.DisplayDialog("Ошибка",
                    "Можно удалять только подпапки корневой папки конфигов.", "OK");
                return;
            }

            var settings = AddressableAssetSettingsDefaultObject.Settings;

            var guids = AssetDatabase.FindAssets($"t:{typeof(GameConfig).Name}", new[] { normFolder });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var cfg = AssetDatabase.LoadAssetAtPath<GameConfig>(path);
                if (cfg == null) continue;

                if (_selected == cfg)
                    Select(null);

                if (settings != null)
                    settings.RemoveAssetEntry(guid);

                _collection.Remove(cfg);
            }

            OnConfigsChanged?.Invoke();

            FileUtil.DeleteFileOrDirectory(normFolder);
            FileUtil.DeleteFileOrDirectory(normFolder + ".meta");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            OnFolderSelected?.Invoke(string.Empty);
            UpdateList();
        }

        private void OnCreated() {}
        private void OnRenamed() {}
        private void OnDuplicated() {}
        

        private static void EnsureFolders(params string[] folders)
        {
            var currentPath = folders[0];
            for (var i = 1; i < folders.Length; i++)
            {
                var nextFolder = Path.Combine(currentPath, folders[i]);
                if (!AssetDatabase.IsValidFolder(nextFolder))
                    AssetDatabase.CreateFolder(currentPath, folders[i]);

                currentPath = nextFolder;
            }
        }
    }
}