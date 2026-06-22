using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Common.Configs.External;
using UnityEditor;
using UnityEngine;

namespace App.Common.Configs.Editor
{
    public class ConfigEditorWindow : EditorWindow
    {
        private const string ConfigFolder = "Assets/App/Configs";

        // ── State ────────────────────────────────────────────────────────────
        private List<GameConfig> _configs = new();
        private GameConfig       _selectedConfig;
        private SerializedObject _serializedObject;

        private Vector2 _leftScroll;
        private Vector2 _rightScroll;
        private string  _searchQuery = "";

        // Tracks the rect of each drawn config button for context-menu hit-testing
        private readonly Dictionary<GameConfig, Rect> _configRects = new();
        // Rect of the entire left-panel scroll area (for "empty space" RMB)
        private Rect _leftPanelRect;

        // ── Styles ───────────────────────────────────────────────────────────
        private GUIStyle _selectedButtonStyle;
        private GUIStyle _normalButtonStyle;
        private GUIStyle _groupLabelStyle;
        private bool     _stylesInitialized;

        // ── Menu item ────────────────────────────────────────────────────────
        [MenuItem("Tools/Config Editor")]
        private static void Open() => GetWindow<ConfigEditorWindow>("Config Editor");

        // ── Lifecycle ────────────────────────────────────────────────────────
        private void OnEnable() => ReloadConfigs();

        // ── Data ─────────────────────────────────────────────────────────────
        private void ReloadConfigs()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(GameConfig)}", new[] { ConfigFolder });

            _configs = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<GameConfig>(
                    AssetDatabase.GUIDToAssetPath(guid)))
                .Where(x => x != null)
                .OrderBy(x => x.name)
                .ToList();

            if (_selectedConfig != null && !_configs.Contains(_selectedConfig))
                ClearSelection();
        }

        private void ClearSelection()
        {
            _selectedConfig   = null;
            _serializedObject = null;
        }

        private void SelectConfig(GameConfig config)
        {
            _selectedConfig   = config;
            _serializedObject = new SerializedObject(config);
        }

        // ── GUI ──────────────────────────────────────────────────────────────
        private void InitStyles()
        {
            if (_stylesInitialized) return;
            _stylesInitialized = true;

            _normalButtonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                padding   = new RectOffset(8, 4, 3, 3),
            };

            _selectedButtonStyle = new GUIStyle(_normalButtonStyle);
            _selectedButtonStyle.normal.background = _selectedButtonStyle.active.background;
            _selectedButtonStyle.normal.textColor  = Color.white;
            _selectedButtonStyle.fontStyle          = FontStyle.Bold;

            _groupLabelStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                fontStyle = FontStyle.Bold,
                padding   = new RectOffset(4, 0, 4, 2),
            };
        }

        private void OnGUI()
        {
            InitStyles();

            EditorGUILayout.BeginHorizontal();
            {
                DrawLeftPanel();
                DrawVerticalSeparator();
                DrawRightPanel();
            }
            EditorGUILayout.EndHorizontal();

            HandleLeftPanelContextMenu();
        }

        // ── Left panel ───────────────────────────────────────────────────────
        private void DrawLeftPanel()
        {
            float panelWidth = Mathf.Max(160f, position.width * 0.28f);

            EditorGUILayout.BeginVertical(GUILayout.Width(panelWidth));
            {
                DrawSearchBar();
                DrawConfigList(panelWidth);
                DrawLeftFooter();
            }
            EditorGUILayout.EndVertical();

            // Capture the full left panel rect after layout
            if (Event.current.type == EventType.Repaint)
                _leftPanelRect = GUILayoutUtility.GetLastRect();
        }

        private void DrawSearchBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.Label("Search", GUILayout.Width(48));
                _searchQuery = EditorGUILayout.TextField(_searchQuery, EditorStyles.toolbarSearchField);

                if (!string.IsNullOrEmpty(_searchQuery) &&
                    GUILayout.Button("✕", EditorStyles.toolbarButton, GUILayout.Width(20)))
                {
                    _searchQuery = "";
                    GUI.FocusControl(null);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawConfigList(float panelWidth)
        {
            _leftScroll = EditorGUILayout.BeginScrollView(_leftScroll);
            {
                _configRects.Clear();

                IEnumerable<GameConfig> visible = string.IsNullOrWhiteSpace(_searchQuery)
                    ? _configs
                    : _configs.Where(c =>
                        c.name.IndexOf(_searchQuery, StringComparison.OrdinalIgnoreCase) >= 0);

                var groups = visible
                    .GroupBy(GetGroupName)
                    .OrderBy(g => g.Key)
                    .ToList();

                bool anyDrawn = false;
                foreach (var group in groups)
                {
                    anyDrawn = true;

                    if (group.Key != string.Empty || groups.Count > 1)
                        EditorGUILayout.LabelField(group.Key, _groupLabelStyle);

                    foreach (var config in group)
                        DrawConfigButton(config, panelWidth);
                }

                if (!anyDrawn)
                {
                    GUILayout.Space(8);
                    EditorGUILayout.HelpBox("No configs found.", MessageType.None);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawConfigButton(GameConfig config, float panelWidth)
        {
            bool     isSelected = _selectedConfig == config;
            GUIStyle style      = isSelected ? _selectedButtonStyle : _normalButtonStyle;

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(config.name, style))
                {
                    if (!isSelected)
                        SelectConfig(config);
                }

                if (GUILayout.Button("◎", EditorStyles.miniButton, GUILayout.Width(22)))
                    EditorGUIUtility.PingObject(config);
            }
            EditorGUILayout.EndHorizontal();

            // Record button rect for context menu hit-testing
            if (Event.current.type == EventType.Repaint)
                _configRects[config] = GUILayoutUtility.GetLastRect();

            // Type subtitle
            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUI.Label(
                new Rect(lastRect.x + 8, lastRect.y - 14, panelWidth - 36, 12),
                config.GetType().Name,
                EditorStyles.centeredGreyMiniLabel);
        }

        private void DrawLeftFooter()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.Label($"{_configs.Count} configs", EditorStyles.miniLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
                    ReloadConfigs();
            }
            EditorGUILayout.EndHorizontal();
        }

        // ── Context menu ─────────────────────────────────────────────────────
        private void HandleLeftPanelContextMenu()
        {
            Event e = Event.current;
            if (e.type != EventType.ContextClick) return;
            if (!_leftPanelRect.Contains(e.mousePosition)) return;

            // Check if the click landed on a specific config button
            GameConfig clickedConfig = null;
            foreach (var kv in _configRects)
            {
                if (kv.Value.Contains(e.mousePosition))
                {
                    clickedConfig = kv.Key;
                    break;
                }
            }

            var menu = new GenericMenu();

            if (clickedConfig != null)
            {
                // RMB on a config item
                menu.AddItem(new GUIContent("Create Config"), false, () => ShowCreateConfigDialog(clickedConfig));
                menu.AddItem(new GUIContent("Create Folder"), false, () => ShowCreateFolderDialog(clickedConfig));
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Rename"),        false, () => ShowRenameDialog(clickedConfig));
                menu.AddItem(new GUIContent("Delete"),        false, () => ConfirmAndDelete(clickedConfig));
            }
            else
            {
                // RMB on empty space in the left panel
                menu.AddItem(new GUIContent("Create Config"), false, () => ShowCreateConfigDialog(null));
                menu.AddItem(new GUIContent("Create Folder"), false, () => ShowCreateFolderDialog(null));
            }

            menu.ShowAsContext();
            e.Use();
        }

        // ── Create Config ─────────────────────────────────────────────────────
        private void ShowCreateConfigDialog(GameConfig nearConfig)
        {
            // Collect all non-abstract GameConfig subtypes in the project
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => { try { return a.GetTypes(); } catch { return Array.Empty<Type>(); } })
                .Where(t => t.IsSubclassOf(typeof(GameConfig)) && !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToList();

            if (types.Count == 0)
            {
                EditorUtility.DisplayDialog("No Config Types",
                    "No concrete GameConfig subclasses found in the project.", "OK");
                return;
            }

            string targetFolder = nearConfig != null
                ? Path.GetDirectoryName(AssetDatabase.GetAssetPath(nearConfig))
                : ConfigFolder;

            CreateConfigDialog.Show(types, targetFolder, onCreate: (type, assetName, folder) =>
            {
                CreateConfig(type, assetName, folder);
            });
        }

        private void CreateConfig(Type type, string assetName, string folder)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                Debug.LogError($"[ConfigEditor] Folder does not exist: {folder}");
                return;
            }

            string path  = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{assetName}.asset");
            var    asset = CreateInstance(type) as GameConfig;

            if (asset == null)
            {
                Debug.LogError($"[ConfigEditor] Failed to create instance of {type.Name}");
                return;
            }

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            ReloadConfigs();
            SelectConfig(asset);
            EditorGUIUtility.PingObject(asset);
        }

        // ── Create Folder ─────────────────────────────────────────────────────
        private void ShowCreateFolderDialog(GameConfig nearConfig)
        {
            string parentFolder = nearConfig != null
                ? Path.GetDirectoryName(AssetDatabase.GetAssetPath(nearConfig))
                : ConfigFolder;

            StringInputDialog.Show(
                title:       "Create Folder",
                label:       "Folder name:",
                defaultValue: "NewFolder",
                onConfirm: folderName =>
                {
                    folderName = folderName.Trim();
                    if (string.IsNullOrEmpty(folderName)) return;

                    string newPath = $"{parentFolder}/{folderName}";
                    if (AssetDatabase.IsValidFolder(newPath))
                    {
                        EditorUtility.DisplayDialog("Folder Exists",
                            $"Folder already exists:\n{newPath}", "OK");
                        return;
                    }

                    AssetDatabase.CreateFolder(parentFolder, folderName);
                    AssetDatabase.Refresh();
                });
        }

        // ── Rename ────────────────────────────────────────────────────────────
        private void ShowRenameDialog(GameConfig config)
        {
            StringInputDialog.Show(
                title:        "Rename Config",
                label:        "New name:",
                defaultValue: config.name,
                onConfirm: newName =>
                {
                    newName = newName.Trim();
                    if (string.IsNullOrEmpty(newName) || newName == config.name) return;

                    string path  = AssetDatabase.GetAssetPath(config);
                    string error = AssetDatabase.RenameAsset(path, newName);

                    if (!string.IsNullOrEmpty(error))
                        Debug.LogError($"[ConfigEditor] Rename failed: {error}");
                    else
                        ReloadConfigs();
                });
        }

        // ── Delete ────────────────────────────────────────────────────────────
        private void ConfirmAndDelete(GameConfig config)
        {
            bool confirmed = EditorUtility.DisplayDialog(
                "Delete Config",
                $"Delete \"{config.name}\"?\nThis cannot be undone.",
                "Delete", "Cancel");

            if (!confirmed) return;

            string path = AssetDatabase.GetAssetPath(config);

            if (_selectedConfig == config)
                ClearSelection();

            AssetDatabase.DeleteAsset(path);
            ReloadConfigs();
        }

        // ── Separator ────────────────────────────────────────────────────────
        private static void DrawVerticalSeparator()
        {
            Rect rect = EditorGUILayout.GetControlRect(false,
                GUILayout.Width(1), GUILayout.ExpandHeight(true));
            EditorGUI.DrawRect(rect, new Color(0, 0, 0, 0.25f));
        }

        // ── Right panel ──────────────────────────────────────────────────────
        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical();
            {
                if (_selectedConfig == null)
                    DrawEmptyState();
                else
                {
                    DrawConfigHeader();
                    DrawInspector();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawEmptyState()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.HelpBox("← Select a config to edit", MessageType.Info);
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }

        private void DrawConfigHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.Label(_selectedConfig.name, EditorStyles.boldLabel);
                GUILayout.Label($"({_selectedConfig.GetType().Name})", EditorStyles.miniLabel);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Select in Project", EditorStyles.toolbarButton))
                {
                    Selection.activeObject = _selectedConfig;
                    EditorGUIUtility.PingObject(_selectedConfig);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawInspector()
        {
            _rightScroll = EditorGUILayout.BeginScrollView(_rightScroll);
            {
                _serializedObject.Update();

                SerializedProperty iterator    = _serializedObject.GetIterator();
                bool               enterChildren = true;

                while (iterator.NextVisible(enterChildren))
                {
                    enterChildren = false;

                    if (iterator.name == "m_Script")
                    {
                        using (new EditorGUI.DisabledScope(true))
                            EditorGUILayout.PropertyField(iterator);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(iterator, includeChildren: true);
                    }
                }

                _serializedObject.ApplyModifiedProperties();
                GUILayout.Space(16);
            }
            EditorGUILayout.EndScrollView();
        }

        // ── Helpers ──────────────────────────────────────────────────────────
        private static string GetGroupName(GameConfig config)
        {
            string path    = AssetDatabase.GetAssetPath(config);
            string trimmed = path.Replace(ConfigFolder + "/", "");
            int    slash   = trimmed.IndexOf('/');
            return slash >= 0 ? trimmed.Substring(0, slash) : string.Empty;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // Helper: modal dialog to pick a GameConfig type + enter an asset name
    // ═══════════════════════════════════════════════════════════════════════════
    internal class CreateConfigDialog : EditorWindow
    {
        private List<Type>              _types;
        private string                  _targetFolder;
        private Action<Type, string, string> _onCreate;

        private int    _selectedTypeIndex;
        private string _assetName = "NewConfig";
        private Vector2 _typeScroll;

        public static void Show(List<Type> types, string targetFolder,
            Action<Type, string, string> onCreate)
        {
            var win = CreateInstance<CreateConfigDialog>();
            win.titleContent  = new GUIContent("Create Config");
            win._types        = types;
            win._targetFolder = targetFolder;
            win._onCreate     = onCreate;
            win._assetName    = "NewConfig";
            win.ShowModal();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select type:", EditorStyles.boldLabel);

            _typeScroll = EditorGUILayout.BeginScrollView(_typeScroll,
                GUILayout.Height(Mathf.Min(_types.Count * 20f + 8f, 200f)));
            {
                for (int i = 0; i < _types.Count; i++)
                {
                    bool selected = _selectedTypeIndex == i;
                    if (GUILayout.Toggle(selected, _types[i].Name, "Button") && !selected)
                    {
                        _selectedTypeIndex = i;
                        if (_assetName == "NewConfig" || _types.Any(t => t.Name == _assetName))
                            _assetName = _types[i].Name;
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            GUILayout.Space(6);
            EditorGUILayout.LabelField("Asset name:", EditorStyles.boldLabel);
            _assetName = EditorGUILayout.TextField(_assetName);

            GUILayout.Space(6);
            EditorGUILayout.LabelField($"Folder: {_targetFolder}", EditorStyles.miniLabel);

            GUILayout.Space(8);
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Create"))
                {
                    string name = _assetName.Trim();
                    if (!string.IsNullOrEmpty(name))
                    {
                        _onCreate?.Invoke(_types[_selectedTypeIndex], name, _targetFolder);
                        Close();
                    }
                }
                if (GUILayout.Button("Cancel"))
                    Close();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // Helper: single-field string input dialog (used for Rename & Create Folder)
    // ═══════════════════════════════════════════════════════════════════════════
    internal class StringInputDialog : EditorWindow
    {
        private string        _label;
        private string        _value;
        private Action<string> _onConfirm;
        private bool          _focusSet;

        public static void Show(string title, string label, string defaultValue,
            Action<string> onConfirm)
        {
            var win = CreateInstance<StringInputDialog>();
            win.titleContent = new GUIContent(title);
            win._label       = label;
            win._value       = defaultValue;
            win._onConfirm   = onConfirm;
            win._focusSet    = false;
            win.minSize      = new Vector2(320, 90);
            win.maxSize      = new Vector2(320, 90);
            win.ShowModal();
        }

        private void OnGUI()
        {
            GUILayout.Space(8);
            EditorGUILayout.LabelField(_label);

            GUI.SetNextControlName("InputField");
            _value = EditorGUILayout.TextField(_value);

            if (!_focusSet)
            {
                EditorGUI.FocusTextInControl("InputField");
                _focusSet = true;
            }

            // Confirm on Enter
            if (Event.current.type == EventType.KeyDown &&
                Event.current.keyCode == KeyCode.Return)
            {
                Confirm();
                return;
            }

            GUILayout.Space(6);
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("OK"))     Confirm();
                if (GUILayout.Button("Cancel")) Close();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void Confirm()
        {
            _onConfirm?.Invoke(_value);
            Close();
        }
    }
}