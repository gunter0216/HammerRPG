using App.Common.Configs.Editor.Right;
using App.Common.Configs.External;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.UIElements.Cursor;

namespace App.Common.Configs.Editor
{
    public class ConfigEditorWindow : EditorWindow
    {
        internal const string ConfigFolder = "Assets/App/Configs";

        // ── Splitter ──────────────────────────────────────────────────────────
        private const float SplitterWidth = 4f;
        private const float MinPanelWidth = 120f;
        private float       _splitterX    = 240f;
        private bool        _isDraggingSplitter;

        // ── IMGUI sub-components (left panel) ─────────────────────────────────
        private LeftPanel         _leftPanel;
        private ConfigsStyle      _configsStyle;
        private RightPanel        _rightPanel;
        private RightClickHandler _rightClickHandler;

        // ── UIElements (right panel) ──────────────────────────────────────────
        private VisualElement    _rightContainer;   // holds header + scroll
        private VisualElement    _rightHeader;      // toolbar row
        private ScrollView       _rightScroll;      // scrollable inspector area
        private InspectorElement _inspector;        // bound to SerializedObject
        private Label            _emptyLabel;       // shown when nothing selected

        // ── Lifecycle ─────────────────────────────────────────────────────────
        [MenuItem("Tools/Config Editor")]
        private static void Open() => GetWindow<ConfigEditorWindow>("Config Editor");

        private void OnEnable()
        {
            InitIMGUI();
            BuildUIElements();
            _leftPanel.ReloadConfigs();
        }

        private void InitIMGUI()
        {
            _rightPanel        ??= new RightPanel();
            _configsStyle      ??= new ConfigsStyle();
            _leftPanel         ??= new LeftPanel(_configsStyle);
            _rightClickHandler   = new RightClickHandler(_leftPanel);
        }

        // ── Build UIElements tree ─────────────────────────────────────────────
        private void BuildUIElements()
        {
            rootVisualElement.Clear();

            // Root: horizontal split  [imgui left | splitter | ue right]
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.flexGrow      = 1;
            rootVisualElement.Add(root);

            // ── Left: IMGUI container ─────────────────────────────────────────
            var leftContainer = new IMGUIContainer(OnIMGUI);
            leftContainer.style.width   = _splitterX;
            leftContainer.style.flexShrink = 0;
            leftContainer.name = "left-imgui";
            root.Add(leftContainer);

            // ── Splitter handle ───────────────────────────────────────────────
            var splitter = new VisualElement();
            splitter.style.width           = SplitterWidth;
            splitter.style.backgroundColor = new Color(0f, 0f, 0f, 0.25f);
            splitter.style.cursor          = new StyleCursor(new Cursor
                { texture = null, hotspot = Vector2.zero });
            splitter.name = "splitter";
            RegisterSplitterEvents(splitter, leftContainer);
            root.Add(splitter);

            // ── Right: UIElements panel ───────────────────────────────────────
            _rightContainer = new VisualElement();
            _rightContainer.style.flexGrow      = 1;
            _rightContainer.style.flexDirection = FlexDirection.Column;
            root.Add(_rightContainer);

            // Header toolbar (hidden until a config is selected)
            _rightHeader = new VisualElement();
            _rightHeader.style.flexDirection  = FlexDirection.Row;
            _rightHeader.style.height         = 21;
            _rightHeader.style.flexShrink     = 0;
            _rightHeader.style.display        = DisplayStyle.None;
            _rightContainer.Add(_rightHeader);

            // Empty-state label
            _emptyLabel = new Label("← Select a config to edit");
            _emptyLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            _emptyLabel.style.flexGrow       = 1;
            _emptyLabel.style.color          = new Color(0.6f, 0.6f, 0.6f);
            _rightContainer.Add(_emptyLabel);

            // ScrollView — grows to fill remaining height automatically
            _rightScroll = new ScrollView(ScrollViewMode.Vertical);
            _rightScroll.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            _rightScroll.style.flexGrow  = 1;
            _rightScroll.style.display   = DisplayStyle.None;
            _rightContainer.Add(_rightScroll);

            // InspectorElement lives inside the scroll
            _inspector = new InspectorElement();
            _inspector.style.flexGrow = 1;
            _rightScroll.Add(_inspector);
        }

        // ── Splitter drag via UIElements pointer events ───────────────────────
        private void RegisterSplitterEvents(VisualElement splitter, IMGUIContainer leftContainer)
        {
            splitter.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0) return;
                _isDraggingSplitter = true;
                splitter.CaptureMouse();
                evt.StopPropagation();
            });

            splitter.RegisterCallback<MouseMoveEvent>(evt =>
            {
                if (!_isDraggingSplitter) return;

                _splitterX = Mathf.Clamp(
                    evt.mousePosition.x,
                    MinPanelWidth,
                    rootVisualElement.layout.width - MinPanelWidth - SplitterWidth);

                leftContainer.style.width = _splitterX;
                evt.StopPropagation();
            });

            splitter.RegisterCallback<MouseUpEvent>(evt =>
            {
                if (!_isDraggingSplitter) return;
                _isDraggingSplitter = false;
                splitter.ReleaseMouse();
                evt.StopPropagation();
            });

            // Resize cursor
            splitter.RegisterCallback<MouseEnterEvent>(_ =>
                splitter.style.cursor = new StyleCursor(
                    new Cursor { texture = null, hotspot = Vector2.zero }));
        }

        // ── IMGUI callback (only left panel) ──────────────────────────────────
        private void OnIMGUI()
        {
            if (_configsStyle == null) InitIMGUI();

            _configsStyle.OnGUI();
            _leftPanel.PanelWidth = _splitterX;

            // Draw the full left panel; position rect width is used only for
            // proportional calculations inside DrawLeftPanel — pass a fake rect.
            _leftPanel.DrawLeftPanel(new Rect(0, 0, _splitterX, position.height));

            _rightClickHandler.HandleContextMenu();

            // Sync right-panel UIElements whenever selection changes
            SyncRightPanel();
        }

        // ── Sync right UIElements panel with current IMGUI selection ──────────
        private GameConfig _lastSynced;

        private void SyncRightPanel()
        {
            var selected = _leftPanel.SelectedConfig;
            if (selected == _lastSynced) return;
            _lastSynced = selected;

            if (selected == null)
            {
                _rightHeader.style.display  = DisplayStyle.None;
                _rightScroll.style.display  = DisplayStyle.None;
                _emptyLabel.style.display   = DisplayStyle.Flex;
                _inspector.Unbind();
            }
            else
            {
                _emptyLabel.style.display   = DisplayStyle.None;
                _rightHeader.style.display  = DisplayStyle.Flex;
                _rightScroll.style.display  = DisplayStyle.Flex;

                RebuildHeader(selected);

                _inspector.Unbind();
                _inspector.Bind(_leftPanel.SerializedObject);
            }
        }

        // ── Right header toolbar ──────────────────────────────────────────────
        private void RebuildHeader(GameConfig config)
        {
            _rightHeader.Clear();

            // Name + type labels
            var nameLabel = new Label(config.name);
            nameLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            nameLabel.style.unityTextAlign          = TextAnchor.MiddleLeft;
            nameLabel.style.marginLeft              = 4;
            _rightHeader.Add(nameLabel);

            var typeLabel = new Label($"({config.GetType().Name})");
            typeLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            typeLabel.style.marginLeft     = 4;
            typeLabel.style.color          = new Color(0.6f, 0.6f, 0.6f);
            _rightHeader.Add(typeLabel);

            // Spacer
            var spacer = new VisualElement();
            spacer.style.flexGrow = 1;
            _rightHeader.Add(spacer);

            // "Select in Project" button
            var btn = new Button(() =>
            {
                Selection.activeObject = config;
                EditorGUIUtility.PingObject(config);
            });
            btn.text = "Select in Project";
            btn.style.marginRight = 4;
            _rightHeader.Add(btn);
        }
    }
}