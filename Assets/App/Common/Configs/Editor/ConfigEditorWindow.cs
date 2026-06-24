using App.Common.Configs.Editor.Right;
using App.Common.Configs.External;
using UnityEditor;
using UnityEngine;

namespace App.Common.Configs.Editor
{
    public class ConfigEditorWindow : EditorWindow
    {
        internal const string ConfigFolder = "Assets/App/Configs";

        // ── Splitter state ────────────────────────────────────────────────────
        private const float SplitterWidth    = 4f;
        private const float MinPanelWidth    = 120f;
        private float       _splitterX       = 240f;
        private bool        _isDraggingSplitter;

        // ── Sub-components ────────────────────────────────────────────────────
        private Vector2           _rightScroll;
        private LeftPanel         _leftPanel;
        private ConfigsStyle      _configsStyle;
        private RightPanel        _rightPanel;
        private RightClickHandler _rightClickHandler;

        [MenuItem("Tools/Config Editor")]
        private static void Open() => GetWindow<ConfigEditorWindow>("Config Editor");

        private void OnEnable()
        {
            Initialize();
            _leftPanel.ReloadConfigs();
        }

        private void Initialize()
        {
            _rightPanel        ??= new RightPanel();
            _configsStyle      ??= new ConfigsStyle();
            _leftPanel         ??= new LeftPanel(_configsStyle);
            _rightClickHandler   = new RightClickHandler(_leftPanel);
        }

        private void OnGUI()
        {
            Initialize();
            _configsStyle.OnGUI();

            // Apply current splitter width to left panel
            _leftPanel.PanelWidth = _splitterX;

            HandleSplitterEvents();

            EditorGUILayout.BeginHorizontal();
            {
                _leftPanel.DrawLeftPanel(position);
                DrawSplitter();
                DrawRightPanel();
            }
            EditorGUILayout.EndHorizontal();

            _rightClickHandler.HandleContextMenu();
        }

        // ── Splitter ──────────────────────────────────────────────────────────
        private void DrawSplitter()
        {
            Rect splitterRect = EditorGUILayout.GetControlRect(
                false,
                GUILayout.Width(SplitterWidth),
                GUILayout.ExpandHeight(true));

            EditorGUI.DrawRect(splitterRect, new Color(0f, 0f, 0f, 0.25f));

            // Change cursor when hovering
            EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeHorizontal);
        }

        private void HandleSplitterEvents()
        {
            Event e = Event.current;

            // The splitter rect is at x = _splitterX, we use a slightly wider
            // hit area for easier grabbing.
            Rect hitRect = new Rect(_splitterX - 3f, 0f, SplitterWidth + 6f, position.height);

            switch (e.type)
            {
                case EventType.MouseDown when hitRect.Contains(e.mousePosition) && e.button == 0:
                    _isDraggingSplitter = true;
                    e.Use();
                    break;

                case EventType.MouseDrag when _isDraggingSplitter:
                    _splitterX = Mathf.Clamp(
                        e.mousePosition.x,
                        MinPanelWidth,
                        position.width - MinPanelWidth - SplitterWidth);
                    Repaint();
                    e.Use();
                    break;

                case EventType.MouseUp when _isDraggingSplitter:
                    _isDraggingSplitter = false;
                    e.Use();
                    break;
            }
        }

        // ── Right panel ───────────────────────────────────────────────────────
        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical();
            {
                if (_leftPanel.SelectedConfig == null)
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
                var selectedConfig = _leftPanel.SelectedConfig;
                GUILayout.Label(selectedConfig.name, EditorStyles.boldLabel);
                GUILayout.Label($"({selectedConfig.GetType().Name})", EditorStyles.miniLabel);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Select in Project", EditorStyles.toolbarButton))
                {
                    Selection.activeObject = selectedConfig;
                    EditorGUIUtility.PingObject(selectedConfig);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawInspector()
        {
            _rightScroll = EditorGUILayout.BeginScrollView(_rightScroll);
            {
                _leftPanel.SerializedObject.Update();

                SerializedProperty iterator      = _leftPanel.SerializedObject.GetIterator();
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

                _leftPanel.SerializedObject.ApplyModifiedProperties();
                GUILayout.Space(16);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
