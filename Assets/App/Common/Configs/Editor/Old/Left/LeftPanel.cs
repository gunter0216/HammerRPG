using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Common.Configs.External;
using UnityEditor;
using UnityEngine;

namespace App.Common.Configs.Editor
{
    public class LeftPanel
    {
        private readonly ConfigsStyle _configsStyle;
        internal string ConfigFolder => ConfigEditorWindow.ConfigFolder;

        // ── Public rects / maps ──────────────────────────────────────────────
        public Rect LeftPanelRect  => _leftPanelRect;

        /// <summary>Rect of each config row (for RMB hit-testing).</summary>
        public Dictionary<GameConfig, Rect> ConfigRects  => _configRects;

        /// <summary>Rect of each folder row (for RMB hit-testing).</summary>
        public Dictionary<string, Rect> FolderRects => _folderRects;

        // ── Public state ─────────────────────────────────────────────────────
        public GameConfig     SelectedConfig   => _selectedConfig;
        public SerializedObject SerializedObject => _serializedObject;

        // ── Panel width (controlled by splitter in ConfigEditorWindow) ───────
        public float PanelWidth { get; set; } = 240f;

        // ── Private ──────────────────────────────────────────────────────────
        private Vector2 _leftScroll;
        private TreeNode _rootNode;

        private readonly Dictionary<GameConfig, Rect> _configRects = new();
        private readonly Dictionary<string, Rect>     _folderRects = new();
        private Rect _leftPanelRect;

        private GameConfig       _selectedConfig;
        
        private string _selectedFolderPath;

        public string SelectedFolderPath => _selectedFolderPath;
        public bool HasFolderSelection => !string.IsNullOrEmpty(_selectedFolderPath);
        
        private List<GameConfig> _configs = new();
        private SerializedObject _serializedObject;

        private string _searchQuery = "";

        public LeftPanel(ConfigsStyle configsStyle)
        {
            _configsStyle = configsStyle;
        }

        // ── Data ─────────────────────────────────────────────────────────────
        public void ReloadConfigs()
        {
            _configs.Clear();

            if (!Directory.Exists(ConfigEditorWindow.ConfigFolder))
            {
                BuildTree();
                return;
            }

            string[] assetPaths = Directory.GetFiles(
                ConfigEditorWindow.ConfigFolder, "*.asset",
                SearchOption.AllDirectories);

            foreach (string path in assetPaths)
            {
                var config = AssetDatabase.LoadAssetAtPath<GameConfig>(
                    path.Replace("\\", "/"));
                if (config != null)
                    _configs.Add(config);
            }

            BuildTree();

            if (SelectedConfig != null && !_configs.Contains(SelectedConfig))
                ClearSelection();
        }

        internal void ClearSelection()
        {
            _selectedConfig = null;
            _selectedFolderPath = null;
            _serializedObject = null;
        }
        
        public void SelectFolder(string folderPath)
        {
            _selectedFolderPath = folderPath;
            _selectedConfig = null;
            _serializedObject = null;
        }

        // ── Tree building ─────────────────────────────────────────────────────
        private void BuildTree()
        {
            _rootNode = new TreeNode
            {
                Name     = "Configs",
                Path     = ConfigFolder,
                IsFolder = true,
                Expanded = true
            };

            // 1. Add all physical sub-folders (so empty ones appear too)
            if (Directory.Exists(ConfigFolder))
                AddFoldersRecursive(_rootNode, ConfigFolder);

            // 2. Add config assets into the correct folder nodes
            foreach (var config in _configs)
            {
                string assetPath     = AssetDatabase.GetAssetPath(config);
                string relativePath  = assetPath.Substring(ConfigFolder.Length).TrimStart('/');
                string[] parts       = relativePath.Split('/');

                TreeNode current = _rootNode;
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    string folderName = parts[i];
                    TreeNode folder = current.Children
                        .FirstOrDefault(x => x.IsFolder && x.Name == folderName);

                    // Should already exist from step 1, but guard just in case
                    if (folder == null)
                    {
                        folder = new TreeNode
                        {
                            Name     = folderName,
                            Path     = current.Path + "/" + folderName,
                            IsFolder = true,
                            Expanded = true
                        };
                        current.Children.Add(folder);
                    }
                    current = folder;
                }

                current.Children.Add(new TreeNode
                {
                    Name     = config.name,
                    Path     = AssetDatabase.GetAssetPath(config),
                    Config   = config,
                    IsFolder = false
                });
            }

            SortTree(_rootNode);
        }

        /// <summary>Recursively creates folder nodes for every sub-directory.</summary>
        private void AddFoldersRecursive(TreeNode parentNode, string physicalPath)
        {
            string[] subDirs = Directory.GetDirectories(physicalPath);
            foreach (string dir in subDirs)
            {
                string name       = Path.GetFileName(dir);
                string assetPath  = dir.Replace("\\", "/");

                // Normalise to asset-db style path
                if (assetPath.StartsWith(Application.dataPath.Replace("\\", "/")))
                    assetPath = "Assets" + assetPath.Substring(Application.dataPath.Length);

                var folderNode = new TreeNode
                {
                    Name     = name,
                    Path     = assetPath,
                    IsFolder = true,
                    Expanded = true
                };
                parentNode.Children.Add(folderNode);
                AddFoldersRecursive(folderNode, dir);
            }
        }

        private void SortTree(TreeNode node)
        {
            node.Children = node.Children
                .OrderByDescending(x => x.IsFolder)
                .ThenBy(x => x.Name)
                .ToList();

            foreach (var child in node.Children.Where(c => c.IsFolder))
                SortTree(child);
        }

        // ── Drawing ───────────────────────────────────────────────────────────
        internal void DrawLeftPanel(Rect windowPosition)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(PanelWidth));
            {
                DrawSearchBar();
                DrawConfigList();
                DrawLeftFooter();
            }
            EditorGUILayout.EndVertical();

            if (Event.current.type == EventType.Repaint)
                _leftPanelRect = GUILayoutUtility.GetLastRect();
        }

        internal void SelectConfig(GameConfig config)
        {
            _selectedFolderPath = null;
            _selectedConfig = config;
            _serializedObject = new SerializedObject(config);
        }

        private void DrawSearchBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.Label("Search", GUILayout.Width(48));
                _searchQuery = EditorGUILayout.TextField(
                    _searchQuery, EditorStyles.toolbarSearchField);

                if (!string.IsNullOrEmpty(_searchQuery) &&
                    GUILayout.Button("✕", EditorStyles.toolbarButton, GUILayout.Width(20)))
                {
                    _searchQuery = "";
                    GUI.FocusControl(null);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawConfigList()
        {
            _configRects.Clear();
            _folderRects.Clear();

            _leftScroll = EditorGUILayout.BeginScrollView(_leftScroll);

            if (_rootNode != null)
            {
                bool filtering = !string.IsNullOrEmpty(_searchQuery);
                if (filtering)
                    DrawFilteredNodes(_rootNode);
                else
                    foreach (var child in _rootNode.Children)
                        DrawTreeNode(child, 0);
            }

            EditorGUILayout.EndScrollView();
        }

        /// <summary>Flat list of matching configs when search is active.</summary>
        private void DrawFilteredNodes(TreeNode node)
        {
            if (!node.IsFolder)
            {
                if (node.Name.IndexOf(_searchQuery, System.StringComparison.OrdinalIgnoreCase) >= 0)
                    DrawConfigRow(node, 0);
                return;
            }
            foreach (var child in node.Children)
                DrawFilteredNodes(child);
        }

        private void DrawTreeNode(TreeNode node, int indent)
        {
            if (node.IsFolder)
                DrawFolderRow(node, indent);
            else
                DrawConfigRow(node, indent);
        }

        // ── Folder row ────────────────────────────────────────────────────────
        private void DrawFolderRow(TreeNode node, int indent)
        {
            bool isSelected = SelectedFolderPath == node.Path;
            
            EditorGUILayout.BeginHorizontal(GUILayout.Height(18));
            GUILayout.Space(indent * 14f);

            // Foldout triangle
            float arrowSize = 22f;
            Rect  arrowRect = GUILayoutUtility.GetRect(arrowSize, 22f,
                GUILayout.Width(arrowSize), GUILayout.Height(22));

            if (Event.current.type == EventType.Repaint)
            {
                string arrow = node.Expanded ? "▾" : "▸";
                EditorStyles.label.Draw(arrowRect,
                    new GUIContent(arrow), false, false, false, false);
            }
            if (Event.current.type == EventType.MouseDown &&
                arrowRect.Contains(Event.current.mousePosition))
            {
                node.Expanded = !node.Expanded;
                Event.current.Use();
            }
            
            GUILayout.Space(-4);

            // Folder icon + name label
            EditorGUILayout.LabelField(
                EditorGUIUtility.IconContent("Folder Icon"),
                GUILayout.Width(18), GUILayout.Height(18));

            EditorGUILayout.LabelField(node.Name, EditorStyles.boldLabel);

            EditorGUILayout.EndHorizontal();
            
            Rect rowRect = GUILayoutUtility.GetLastRect();

            if (Event.current.type == EventType.Repaint && isSelected)
            {
                EditorGUI.DrawRect(
                    rowRect,
                    new Color(0.24f, 0.49f, 0.91f, 0.35f));
            }

            // Record rect for RMB
            if (Event.current.type == EventType.Repaint)
                _folderRects[node.Path] = rowRect;

            // Whole row clickable to toggle
            if (Event.current.type == EventType.MouseDown &&
                _folderRects.TryGetValue(node.Path, out Rect fr) &&
                fr.Contains(Event.current.mousePosition) &&
                Event.current.button == 0)
            {
                SelectFolder(node.Path);
                Event.current.Use();
            }

            if (!node.Expanded) return;

            foreach (var child in node.Children)
                DrawTreeNode(child, indent + 1);
        }

        // ── Config row ────────────────────────────────────────────────────────
        private void DrawConfigRow(TreeNode node, int indent)
        {
            bool isSelected = SelectedConfig == node.Config;

            EditorGUILayout.BeginHorizontal(GUILayout.Height(18));
            GUILayout.Space(indent * 14f + 16f);   // align past arrow column

            // Highlight selected row
            Rect rowRect = EditorGUILayout.GetControlRect(
                GUILayout.ExpandWidth(true), GUILayout.Height(18));

            if (Event.current.type == EventType.Repaint)
            {
                if (isSelected)
                    EditorGUI.DrawRect(rowRect, new Color(0.24f, 0.49f, 0.91f, 0.35f));

                var labelStyle = isSelected
                    ? _configsStyle.SelectedLabelStyle
                    : _configsStyle.NormalLabelStyle;
                labelStyle.Draw(rowRect,
                    new GUIContent(node.Name), false, false, isSelected, false);

                _configRects[node.Config] = rowRect;
            }

            if (Event.current.type == EventType.MouseDown &&
                rowRect.Contains(Event.current.mousePosition) &&
                Event.current.button == 0)
            {
                SelectConfig(node.Config);
                Event.current.Use();
            }

            EditorGUILayout.EndHorizontal();
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

        // ── Helper: find folder node by path ─────────────────────────────────
        public TreeNode FindFolderNode(string path)
        {
            return FindFolderNodeRecursive(_rootNode, path);
        }

        private TreeNode FindFolderNodeRecursive(TreeNode node, string path)
        {
            if (node.IsFolder && node.Path == path) return node;
            foreach (var child in node.Children)
            {
                var result = FindFolderNodeRecursive(child, path);
                if (result != null) return result;
            }
            return null;
        }
    }
}
