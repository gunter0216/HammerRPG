using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    internal class TagsToolbarPanel : VisualElement
    {
        private readonly VisualElement _tagsRow;
        private readonly Button _plusButton;

        private List<string> _selectedTags = new();

        public Func<IReadOnlyList<string>> AllTagsProvider { get; private set; }

        public event Action<string> OnTagAdded;
        public event Action<List<string>> OnTagsChanged;
        public event Action<string> OnDeleteTagRequested;

        public TagsToolbarPanel()
        {
            AddToClassList("tags-toolbar-panel");
            
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;

            _tagsRow = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1,
                    flexWrap = Wrap.NoWrap,
                    marginBottom = 6
                }
            };

            _plusButton = new Button(ShowPopup)
            {
                iconImage = Background.FromTexture2D((Texture2D)EditorGUIUtility.IconContent("CustomTool@2x").image),
                tooltip = "Добавить/выбрать теги",
                style =
                {
                    width = 24,
                    height = 24,
                    marginLeft = 6
                }
            };

            _plusButton.style.display = DisplayStyle.None;

            Add(_tagsRow);
            Add(_plusButton);
        }

        public void Bind(List<string> selectedTags, Func<IReadOnlyList<string>> allTagsProvider)
        {
            _selectedTags = selectedTags ?? new List<string>();
            AllTagsProvider = allTagsProvider;
            RebuildTags();
        }

        public void RebuildTags()
        {
            _tagsRow.Clear();
            foreach (var tag in _selectedTags)
            {
                _tagsRow.Add(MakeTagElement(tag));
            }
        }

        private VisualElement MakeTagElement(string tag)
        {
            var tagElement = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    paddingLeft = 8, paddingRight = 4,
                    height = 22,
                    borderTopLeftRadius = 10, borderTopRightRadius = 10,
                    borderBottomLeftRadius = 10, borderBottomRightRadius = 10,
                    borderBottomWidth = 1, borderTopWidth = 1, borderLeftWidth = 1, borderRightWidth = 1,
                    marginRight = 4
                }
            };
            
            tagElement.AddToClassList("tag-element");

            var label = new Label(tag)
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleLeft,
                    marginRight = 4
                }
            };

            var closeBtn = new Button(() => OnDeleteTagRequested?.Invoke(tag))
            {
                text = "×",
                tooltip = $"Удалить тег \"{tag}\" навсегда",
                style =
                {
                    width = 16,
                    height = 16,
                    paddingLeft = 0, paddingRight = 0, paddingTop = 0, paddingBottom = 0,
                    marginLeft = 0, marginRight = 0
                }
            };

            closeBtn.AddToClassList("tag-element-delete-button");
            
            tagElement.Add(label);
            tagElement.Add(closeBtn);
            return tagElement;
        }

        public void SetAddButtonVisible(bool visible)
        {
            _plusButton.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void ShowPopup()
        {
            var rect = _plusButton.worldBound;
            var screenPos = rect.position;
            var popupRect = new Rect(screenPos, new Vector2(0, 0));

            var allTags = AllTagsProvider?.Invoke() ?? Array.Empty<string>();

            TagsPopupWindow.Show(
                popupRect,
                _selectedTags,
                allTags,
                updated =>
                {
                    OnTagsChanged?.Invoke(updated);
                },
                newTag =>
                {
                    if (!string.IsNullOrWhiteSpace(newTag))
                        OnTagAdded?.Invoke(newTag);
                }
            );
        }
    }
}