using System.Collections.Generic;
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

        // ── Styles (lazy-init because EditorStyles aren't ready in constructors)
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

            // Keep selection valid after reload
            if (_selectedConfig != null && !_configs.Contains(_selectedConfig))
                ClearSelection();
        }

        private void ClearSelection()
        {
            _selectedConfig  = null;
            _serializedObject = null;
        }

        private void SelectConfig(GameConfig config)
        {
            _selectedConfig  = config;
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
            _selectedButtonStyle.normal.background  =
                _selectedButtonStyle.active.background;
            _selectedButtonStyle.normal.textColor   = Color.white;
            _selectedButtonStyle.fontStyle           = FontStyle.Bold;

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
        }

        private void DrawSearchBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.Label("Search", GUILayout.Width(48));
                _searchQuery = EditorGUILayout.TextField(_searchQuery,
                    EditorStyles.toolbarSearchField);

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
                IEnumerable<GameConfig> visible = string.IsNullOrWhiteSpace(_searchQuery)
                    ? _configs
                    : _configs.Where(c =>
                        c.name.IndexOf(_searchQuery, System.StringComparison.OrdinalIgnoreCase) >= 0);

                // Group by immediate sub-folder name
                var groups = visible
                    .GroupBy(c => GetGroupName(c))
                    .OrderBy(g => g.Key);

                bool anyDrawn = false;
                foreach (var group in groups)
                {
                    anyDrawn = true;

                    // Draw group header only when there are multiple groups
                    if (group.Key != string.Empty || groups.Count() > 1)
                    {
                        EditorGUILayout.LabelField(group.Key, _groupLabelStyle);
                    }

                    foreach (var config in group)
                    {
                        DrawConfigButton(config, panelWidth);
                    }
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
            bool isSelected = _selectedConfig == config;
            GUIStyle style  = isSelected ? _selectedButtonStyle : _normalButtonStyle;

            // Type name shown as a dim suffix
            string typeName = config.GetType().Name;
            string label    = config.name;

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(label, style))
                {
                    if (!isSelected)
                        SelectConfig(config);
                }

                // Small "ping" button to highlight in Project window
                if (GUILayout.Button("◎", EditorStyles.miniButton, GUILayout.Width(22)))
                    EditorGUIUtility.PingObject(config);
            }
            EditorGUILayout.EndHorizontal();

            // Draw type subtitle
            Rect lastRect = GUILayoutUtility.GetLastRect();
            // (type label sits inside the button rect — drawn after so it overlaps cleanly)
            GUI.Label(
                new Rect(lastRect.x + 8, lastRect.y - 14, panelWidth - 36, 12),
                typeName,
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

        // ── Separator ────────────────────────────────────────────────────────
        private static void DrawVerticalSeparator()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, GUILayout.Width(1),
                GUILayout.ExpandHeight(true));
            EditorGUI.DrawRect(rect, new Color(0, 0, 0, 0.25f));
        }

        // ── Right panel ──────────────────────────────────────────────────────
        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical();
            {
                if (_selectedConfig == null)
                {
                    DrawEmptyState();
                }
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

                SerializedProperty iterator   = _serializedObject.GetIterator();
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

                GUILayout.Space(16); // breathing room at bottom
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
}