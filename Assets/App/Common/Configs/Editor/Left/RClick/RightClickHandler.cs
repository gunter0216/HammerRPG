using System;
using System.IO;
using System.Linq;
using App.Common.Configs.External;
using UnityEditor;
using UnityEngine;

namespace App.Common.Configs.Editor
{
    public class RightClickHandler
    {
        private readonly LeftPanel _leftPanel;

        public RightClickHandler(LeftPanel leftPanel)
        {
            _leftPanel = leftPanel;
        }

        // ── Entry point (called from ConfigEditorWindow.OnGUI) ────────────────
        internal void HandleContextMenu()
        {
            Event e = Event.current;
            if (e.type != EventType.ContextClick) return;
            if (!_leftPanel.LeftPanelRect.Contains(e.mousePosition)) return;

            // 1. Check if click is on a config row
            GameConfig clickedConfig = null;
            foreach (var kv in _leftPanel.ConfigRects)
            {
                if (kv.Value.Contains(e.mousePosition))
                {
                    clickedConfig = kv.Key;
                    break;
                }
            }

            // 2. Check if click is on a folder row
            string clickedFolderPath = null;
            if (clickedConfig == null)
            {
                foreach (var kv in _leftPanel.FolderRects)
                {
                    if (kv.Value.Contains(e.mousePosition))
                    {
                        clickedFolderPath = kv.Key;
                        break;
                    }
                }
            }

            var menu = new GenericMenu();

            if (clickedConfig != null)
            {
                // ── RMB on a config file ──────────────────────────────────────
                menu.AddItem(new GUIContent("Direction"),       false, () => PingConfig(clickedConfig));
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Rename"),          false, () => ShowRenameDialog(clickedConfig));
                menu.AddItem(new GUIContent("Duplicate"),       false, () => DuplicateConfig(clickedConfig));
                menu.AddItem(new GUIContent("Delete"),          false, () => ConfirmAndDelete(clickedConfig));
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("New Config"),      false, () => ShowCreateConfigDialog(GetConfigFolder(clickedConfig)));
                menu.AddItem(new GUIContent("New Folder"),      false, () => ShowCreateFolderDialog(GetConfigFolder(clickedConfig)));
            }
            else if (clickedFolderPath != null)
            {
                // ── RMB on a folder ───────────────────────────────────────────
                menu.AddItem(new GUIContent("New Config"),      false, () => ShowCreateConfigDialog(clickedFolderPath));
                menu.AddItem(new GUIContent("New Folder"),      false, () => ShowCreateFolderDialog(clickedFolderPath));
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Rename Folder"),   false, () => ShowRenameFolderDialog(clickedFolderPath));
                menu.AddItem(new GUIContent("Delete Folder"),   false, () => ConfirmAndDeleteFolder(clickedFolderPath));
            }
            else
            {
                // ── RMB on empty space ────────────────────────────────────────
                menu.AddItem(new GUIContent("New Config"),      false, () => ShowCreateConfigDialog(ConfigEditorWindow.ConfigFolder));
                menu.AddItem(new GUIContent("New Folder"),      false, () => ShowCreateFolderDialog(ConfigEditorWindow.ConfigFolder));
            }

            menu.ShowAsContext();
            e.Use();
        }

        // ── Direction (ping) ──────────────────────────────────────────────────
        private static void PingConfig(GameConfig config)
        {
            Selection.activeObject = config;
            EditorGUIUtility.PingObject(config);
        }

        // ── Create Config ─────────────────────────────────────────────────────
        private void ShowCreateConfigDialog(string targetFolder)
        {
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

            CreateConfigDialog.Show(types, targetFolder, (type, assetName, folder) =>
                CreateConfig(type, assetName, folder));
        }

        private void CreateConfig(Type type, string assetName, string folder)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                Debug.LogError($"[ConfigEditor] Folder does not exist: {folder}");
                return;
            }

            string path  = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{assetName}.asset");
            var    asset = ScriptableObject.CreateInstance(type) as GameConfig;

            if (asset == null)
            {
                Debug.LogError($"[ConfigEditor] Failed to create instance of {type.Name}");
                return;
            }

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            _leftPanel.ReloadConfigs();
            _leftPanel.SelectConfig(asset);
            EditorGUIUtility.PingObject(asset);
        }

        // ── Create Folder ─────────────────────────────────────────────────────
        private void ShowCreateFolderDialog(string parentFolder)
        {
            StringInputDialog.Show(
                title:        "Create Folder",
                label:        "Folder name:",
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
                    _leftPanel.ReloadConfigs();
                });
        }

        // ── Rename Config ─────────────────────────────────────────────────────
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
                        _leftPanel.ReloadConfigs();
                });
        }

        // ── Rename Folder ─────────────────────────────────────────────────────
        private void ShowRenameFolderDialog(string folderPath)
        {
            string currentName = Path.GetFileName(folderPath);
            StringInputDialog.Show(
                title:        "Rename Folder",
                label:        "New name:",
                defaultValue: currentName,
                onConfirm: newName =>
                {
                    newName = newName.Trim();
                    if (string.IsNullOrEmpty(newName) || newName == currentName) return;

                    string error = AssetDatabase.RenameAsset(folderPath, newName);
                    if (!string.IsNullOrEmpty(error))
                        Debug.LogError($"[ConfigEditor] Rename folder failed: {error}");
                    else
                    {
                        AssetDatabase.Refresh();
                        _leftPanel.ReloadConfigs();
                    }
                });
        }

        // ── Duplicate ─────────────────────────────────────────────────────────
        private void DuplicateConfig(GameConfig config)
        {
            string sourcePath = AssetDatabase.GetAssetPath(config);
            string folder     = Path.GetDirectoryName(sourcePath)?.Replace("\\", "/");
            string newPath    = AssetDatabase.GenerateUniqueAssetPath(
                $"{folder}/{config.name}_Copy.asset");

            if (AssetDatabase.CopyAsset(sourcePath, newPath))
            {
                AssetDatabase.Refresh();
                _leftPanel.ReloadConfigs();
                var newAsset = AssetDatabase.LoadAssetAtPath<GameConfig>(newPath);
                if (newAsset != null)
                {
                    _leftPanel.SelectConfig(newAsset);
                    EditorGUIUtility.PingObject(newAsset);
                }
            }
            else
            {
                Debug.LogError($"[ConfigEditor] Duplicate failed: {sourcePath}");
            }
        }

        // ── Delete Config ─────────────────────────────────────────────────────
        private void ConfirmAndDelete(GameConfig config)
        {
            bool confirmed = EditorUtility.DisplayDialog(
                "Delete Config",
                $"Delete \"{config.name}\"?\nThis cannot be undone.",
                "Delete", "Cancel");

            if (!confirmed) return;

            if (_leftPanel.SelectedConfig == config)
                _leftPanel.ClearSelection();

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(config));
            _leftPanel.ReloadConfigs();
        }

        // ── Delete Folder ─────────────────────────────────────────────────────
        private void ConfirmAndDeleteFolder(string folderPath)
        {
            string name = Path.GetFileName(folderPath);
            bool confirmed = EditorUtility.DisplayDialog(
                "Delete Folder",
                $"Delete folder \"{name}\" and all its contents?\nThis cannot be undone.",
                "Delete", "Cancel");

            if (!confirmed) return;

            // Clear selection if selected config is inside this folder
            if (_leftPanel.SelectedConfig != null)
            {
                string selPath = AssetDatabase.GetAssetPath(_leftPanel.SelectedConfig);
                if (selPath.StartsWith(folderPath))
                    _leftPanel.ClearSelection();
            }

            AssetDatabase.DeleteAsset(folderPath);
            AssetDatabase.Refresh();
            _leftPanel.ReloadConfigs();
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private static string GetConfigFolder(GameConfig config)
        {
            string path = AssetDatabase.GetAssetPath(config);
            return Path.GetDirectoryName(path)?.Replace("\\", "/") ?? ConfigEditorWindow.ConfigFolder;
        }
    }
}
