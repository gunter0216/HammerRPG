using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    internal class PropertiesPanel<T> : VisualElement where T : ScriptableObject
    {
        private readonly VisualElement _content;
        private readonly TagsToolbarPanel _tagsToolbar;

        public event Action<string> OnTagAdded;
        public event Action<List<string>> OnTagsChanged;
        public event Action<string> OnDeleteTagRequested;

        public PropertiesPanel(VisualElement content)
        {
            _content = content;
            _tagsToolbar = new TagsToolbarPanel();

            Add(_tagsToolbar);
            Add(_content);

            _tagsToolbar.OnTagAdded += tag => OnTagAdded?.Invoke(tag);
            _tagsToolbar.OnTagsChanged += tags => OnTagsChanged?.Invoke(tags);
            _tagsToolbar.OnDeleteTagRequested += tag => OnDeleteTagRequested?.Invoke(tag);
        }

        public void BindSelectedTags(List<string> selectedTags, Func<IReadOnlyList<string>> allTagsProvider)
        {
            _tagsToolbar.Bind(selectedTags, allTagsProvider);
        }

        public void RefreshTags()
        {
            _tagsToolbar.RebuildTags();
        }

        public void SetAddButtonVisible(bool visible)
        {
            _tagsToolbar.SetAddButtonVisible(visible);
        }
    }
}