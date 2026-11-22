using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    internal partial class ConfigListPanel<T> where T : ScriptableObject
    {
        protected internal class Row
        {
            public bool IsFolder;
            public string FolderPath;
            public string FolderName;
            public bool CanExpand;
            public bool Expanded;
            public int Depth;
            public T Config;
            public string ConfigName;
        }

        protected internal class RowWidgets
        {
            public Button FolderButton;
            public Image Icon;
            public Label Label;
            public DragHandler DragHandler;
            public Row Row;
        }

        private VisualElement MakeRow()
        {
            var root = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    height = RowHeight,
                    paddingLeft = 6,
                    paddingRight = 8
                }
            };

            var fold = new Button { text = "", tooltip = "Expand/Collapse" };
            fold.style.width = 18;
            fold.style.height = 18;
            fold.style.flexShrink = 0;

            var icon = new Image
            {
                name = "ConfigIcon",
                scaleMode = ScaleMode.ScaleToFit,
                pickingMode = PickingMode.Ignore,
                style = { width = 22, height = 22, display = DisplayStyle.None }
            };
            var leading = new VisualElement
            {
                pickingMode = PickingMode.Ignore,
                style =
                {
                    width = 25,
                    flexShrink = 0,
                    alignItems = Align.Center,
                    justifyContent = Justify.Center
                }
            };
            leading.Add(fold);
            leading.Add(icon);

            var label = new Label
            {
                style =
                {
                    flexGrow = 1,
                    overflow = Overflow.Hidden,
                    textOverflow = TextOverflow.Ellipsis,
                    whiteSpace = WhiteSpace.NoWrap
                }
            };

            root.Add(leading);
            root.Add(label);

            root.AddManipulator(new ContextualMenuManipulator(MenuBuilder));

            var widgets = new RowWidgets
            {
                FolderButton = fold,
                Icon = icon,
                Label = label,
                DragHandler = new DragHandler(root)
            };
            root.userData = widgets;
            fold.clicked += () =>
            {
                if (widgets.Row?.CanExpand == true)
                    ToggleFolder(widgets.Row.FolderPath);
            };
            fold.focusable = true;

            return root;

            void MenuBuilder(ContextualMenuPopulateEvent obj)
            {
                obj.menu.AppendAction("Direction", _ =>
                {
                    var widget = (RowWidgets)root.userData;
                    var row = widget.Row;

                    if (!row.IsFolder)
                    {
                        Selection.activeObject = row.Config;
                    }
                });
                
                obj.menu.AppendAction("Rename", _ =>
                {
                    var widget = (RowWidgets)root.userData;
                    var row = widget.Row;

                    if (row.IsFolder) StartFolderInlineEdit(row.FolderPath);
                    else StartConfigInlineEdit(row.Config);
                }, DropdownMenuAction.AlwaysEnabled);

                obj.menu.AppendAction("Duplicate", _ =>
                {
                    var widget = (RowWidgets)root.userData;
                    var row = widget.Row;

                    if (!row.IsFolder)
                        OnDuplicateConfigRequested?.Invoke(row.Config);
                });
                
                obj.menu.AppendAction("Delete", _ =>
                {
                    var widget = (RowWidgets)root.userData;
                    var row = widget.Row;

                    if (row.IsFolder)
                    {
                        var confirm = EditorUtility.DisplayDialog("Удалить папку",
                            $"Действительно удалить папку \"{row.FolderName}\" и все её конфиги?",
                            "Удалить", "Отмена");

                        if (confirm) OnFolderDeleteRequested?.Invoke(row.FolderPath);
                    }
                    else
                    {
                        OnConfigDeleteRequested?.Invoke(row.Config);
                    }
                }, DropdownMenuAction.AlwaysEnabled);

                obj.menu.AppendSeparator();
            }
        }

        private void BindRow(VisualElement element, int index)
        {
            var widgets = (RowWidgets)element.userData;
            var row = _flat[index];

            element.name = row.IsFolder ? row.FolderName : row.ConfigName;
            element.tooltip = element.name;
            element.style.paddingLeft = 6 + row.Depth * Indent;

            widgets.Row = row;
            widgets.DragHandler.Bind(row, this, element);
            BindIcon(widgets, row.Config);

            if (row.IsFolder)
            {
                widgets.FolderButton.style.display = row.CanExpand ? DisplayStyle.Flex : DisplayStyle.None;
                widgets.FolderButton.text = row.Expanded ? "▾" : "▸";
                widgets.Label.style.unityFontStyleAndWeight = FontStyle.Bold;

                if (_editMode == EditMode.Folder && _editingFolderPath == row.FolderPath)
                    EnsureFolderInlineEditor(element, widgets, row);
                else
                    RestoreLabel(element, widgets, row.FolderName);
            }
            else
            {
                widgets.FolderButton.style.display = DisplayStyle.None;
                widgets.FolderButton.text = "";
                widgets.Label.style.unityFontStyleAndWeight = FontStyle.Normal;

                if (_editMode == EditMode.Config && ReferenceEquals(_editingConfig, row.Config))
                    EnsureConfigInlineEditor(element, widgets, row);
                else
                    RestoreLabel(element, widgets, row.ConfigName);
            }
        }

        public void RefreshIcon(T config)
        {
            if (config == null)
                return;

            int index = _flat.FindIndex(row => row.Config == config);
            if (index >= 0 && _listView.GetRootElementForIndex(index)?.userData is RowWidgets widgets)
                BindIcon(widgets, config);
        }

        private void BindIcon(RowWidgets widgets, T config)
        {
            Sprite sprite = config != null ? _getIcon?.Invoke(config) : null;
            widgets.Icon.sprite = sprite;
            widgets.Icon.style.display = sprite != null ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void HandleRowMouseDown(in Row row)
        {
            if (row.IsFolder)
            {
                _listView.selectedIndex = -1;
                OnConfigSelected?.Invoke(null);
                OnFolderSelected?.Invoke(row.FolderPath);
            }
            else
            {
                OnConfigSelected?.Invoke(row.Config);
            }
        }
    }
}
