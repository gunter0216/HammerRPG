using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    internal partial class ConfigListPanel<T> where T : ScriptableObject
    {
        internal class DragHandler
        {
            private Row _row;
            private ConfigListPanel<T> _owner;
            private VisualElement _rootElement;

            public DragHandler(VisualElement root)
            {
                _rootElement = root;
                _rootElement.RegisterCallback<MouseDownEvent>(OnMouseDown);
                _rootElement.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
                _rootElement.RegisterCallback<DragPerformEvent>(OnDragPerform);
                _rootElement.RegisterCallback<DragLeaveEvent>(OnDragLeave);
            }

            public void Bind(Row row, ConfigListPanel<T> owner, VisualElement rootElement)
            {
                _row = row;
                _owner = owner;
                _rootElement = rootElement;
            }

            private void OnMouseDown(MouseDownEvent evt)
            {
                if (evt.button != 0) return;
                if (_owner == null) return;

                _owner.HandleRowMouseDown(_row);

                if (_row.IsFolder || _row.Config == null) return;
                if (_owner._editMode != EditMode.None) return;

                string path = AssetDatabase.GetAssetPath(_row.Config);
                if (string.IsNullOrEmpty(path)) return;

                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new UnityEngine.Object[] { _row.Config };
                DragAndDrop.paths = new[] { path };
                DragAndDrop.SetGenericData(DndId, DragAndDrop.paths);
                DragAndDrop.StartDrag("Move Config");
            }

            private void OnDragUpdated(DragUpdatedEvent evt)
            {
                if (!_row.IsFolder) return;

                var data = DragAndDrop.GetGenericData(DndId) as string[];
                if (data == null || data.Length == 0) return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                _rootElement.style.backgroundColor = new Color(0.2f, 0.4f, 0.7f, 0.20f);
                evt.StopPropagation();
            }

            private void OnDragLeave(DragLeaveEvent evt)
            {
                if (_row.IsFolder)
                    _rootElement.style.backgroundColor = StyleKeyword.Null;
            }

            private void OnDragPerform(DragPerformEvent evt)
            {
                if (!_row.IsFolder) return;

                var data = DragAndDrop.GetGenericData(DndId) as string[];
                if (data == null || data.Length == 0) return;

                DragAndDrop.AcceptDrag();
                evt.StopPropagation();

                _rootElement.style.backgroundColor = StyleKeyword.Null;

                string targetFolder = _row.FolderPath;
                bool anyMoved = false;

                foreach (var srcPath in data)
                {
                    if (string.IsNullOrEmpty(srcPath)) continue;

                    var srcDir = Path.GetDirectoryName(srcPath)?.Replace('\\', '/');
                    if (string.Equals(srcDir, targetFolder, StringComparison.Ordinal))
                        continue;

                    string fileName = Path.GetFileName(srcPath);
                    string destPathCandidate = Path.Combine(targetFolder, fileName).Replace('\\', '/');
                    string destPath = AssetDatabase.GenerateUniqueAssetPath(destPathCandidate);

                    string err = AssetDatabase.MoveAsset(srcPath, destPath);
                    if (!string.IsNullOrEmpty(err))
                    {
                        Debug.LogError($"MoveAsset error: {err}");
                        continue;
                    }
                    anyMoved = true;
                }

                if (anyMoved)
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    _owner.Filter(_owner._searchText, _owner._selectedTags);
                    _owner._expanded.Add(targetFolder);
                    _owner.RebuildFlat();
                    _owner._listView.itemsSource = _owner._flat;
                    _owner._listView.Rebuild();
                }
            }
        }
    }
}
