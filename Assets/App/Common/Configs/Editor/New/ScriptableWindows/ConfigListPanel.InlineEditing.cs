using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    internal partial class ConfigListPanel<T> where T : ScriptableObject
    {
        private void StartConfigInlineEdit(T config)
        {
            _editMode = EditMode.Config;
            _editingConfig = config;
            _listView.Rebuild();

            int idx = _flat.FindIndex(r => !r.IsFolder && Equals(r.Config, config));
            if (idx < 0) return;

            var rowElem = _listView.GetRootElementForIndex(idx);
            if (rowElem == null) return;

            var widgets = (RowWidgets)rowElem.userData;
            EnsureConfigInlineEditor(rowElem, widgets, _flat[idx]);
        }

        private void EnsureConfigInlineEditor(VisualElement rowElem, RowWidgets widgets, in Row row)
        {
            var parent = widgets.Label.parent;
            if (parent == null) return;
            int labelIndex = parent.IndexOf(widgets.Label);
            if (labelIndex < 0) labelIndex = 1;

            if (widgets.Label.parent == parent)
                parent.Remove(widgets.Label);

            _editingField = new TextField
            {
                value = row.ConfigName,
                style = { flexGrow = 1, minHeight = 20, maxHeight = 20 }
            };

            _editingField.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                {
                    ApplyConfigRename();
                    evt.StopPropagation();
                }
                else if (evt.keyCode == KeyCode.Escape)
                {
                    CancelEditing();
                    evt.StopPropagation();
                }
            });
            _editingField.RegisterCallback<BlurEvent>(_ => ApplyConfigRename());

            parent.Insert(labelIndex, _editingField);
            _editingField.Focus();
            _editingField.SelectAll();
        }

        private void ApplyConfigRename()
        {
            if (_editMode != EditMode.Config || _editingConfig == null || _editingField == null)
            {
                CancelEditing();
                return;
            }

            var newName = (_editingField.value ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(newName) || string.Equals(_editingConfig.name, newName))
            {
                CancelEditing();
                return;
            }

            if (_allConfigs.Any(c => c != _editingConfig && c.name == newName))
            {
                EditorUtility.DisplayDialog("Ошибка", "Конфиг с таким именем уже существует.", "OK");
                _editingField.Focus();
                _editingField.SelectAll();
                return;
            }

            OnConfigRenameRequested?.Invoke(_editingConfig, newName);

            Filter(_searchText, _selectedTags);
            CancelEditing();
        }

        private void StartFolderInlineEdit(string folderPath)
        {
            _editMode = EditMode.Folder;
            _editingFolderPath = folderPath;
            _listView.Rebuild();

            int idx = _flat.FindIndex(r => r.IsFolder && r.FolderPath == folderPath);
            if (idx < 0) return;

            var rowElem = _listView.GetRootElementForIndex(idx);
            if (rowElem == null) return;

            var widgets = (RowWidgets)rowElem.userData;
            EnsureFolderInlineEditor(rowElem, widgets, _flat[idx]);
        }

        private void EnsureFolderInlineEditor(VisualElement rowElem, RowWidgets widgets, in Row row)
        {
            var parent = widgets.Label.parent;
            if (parent == null) return;
            int labelIndex = parent.IndexOf(widgets.Label);
            if (labelIndex < 0) labelIndex = 1;

            if (widgets.Label.parent == parent)
                parent.Remove(widgets.Label);

            _editingFolderField = new TextField
            {
                value = row.FolderName,
                style = { flexGrow = 1, minHeight = 20, maxHeight = 20 }
            };

            _editingFolderField.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                {
                    ApplyFolderRename();
                    evt.StopPropagation();
                }
                else if (evt.keyCode == KeyCode.Escape)
                {
                    CancelEditing();
                    evt.StopPropagation();
                }
            });
            _editingFolderField.RegisterCallback<BlurEvent>(_ => ApplyFolderRename());

            parent.Insert(labelIndex, _editingFolderField);
            _editingFolderField.Focus();
            _editingFolderField.SelectAll();
        }

        private void ApplyFolderRename()
        {
            if (_editMode != EditMode.Folder || string.IsNullOrEmpty(_editingFolderPath) || _editingFolderField == null)
            {
                CancelEditing();
                return;
            }

            string newName = (_editingFolderField.value ?? "").Trim();
            string currentName = Path.GetFileName(_editingFolderPath);
            if (string.IsNullOrEmpty(newName) || newName == currentName)
            {
                CancelEditing();
                return;
            }

            string parent = Path.GetDirectoryName(_editingFolderPath).Replace('\\', '/');
            string newPath = $"{parent}/{newName}";

            if (AssetDatabase.IsValidFolder(newPath))
            {
                EditorUtility.DisplayDialog("Переименование", "Папка с таким именем уже существует.", "OK");
                _editingFolderField.Focus();
                _editingFolderField.SelectAll();
                return;
            }

            string err = AssetDatabase.MoveAsset(_editingFolderPath, newPath);
            if (!string.IsNullOrEmpty(err))
            {
                EditorUtility.DisplayDialog("Переименование", $"Не удалось переименовать папку:\n{err}", "OK");
                _editingFolderField.Focus();
                _editingFolderField.SelectAll();
                return;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _expanded.Add(newPath);
            Filter(_searchText, _selectedTags);
            CancelEditing();
        }

        private void CancelEditing()
        {
            _editMode = EditMode.None;
            _editingConfig = null;
            _editingField = null;
            _editingFolderPath = null;
            _editingFolderField = null;
            _listView.Rebuild();
        }

        private static void RestoreLabel(VisualElement rowElement, RowWidgets widgets, string text)
        {
            if (!(widgets.Label.parent is VisualElement parent))
                return;

            if (widgets.Label.parent == parent)
            {
                widgets.Label.text = text;
                return;
            }

            int indexToInsert = 1;
            var existingTF = parent.Q<TextField>();
            if (existingTF != null)
                parent.Remove(existingTF);

            parent.Insert(indexToInsert, widgets.Label);
            widgets.Label.text = text;
        }
    }
}
