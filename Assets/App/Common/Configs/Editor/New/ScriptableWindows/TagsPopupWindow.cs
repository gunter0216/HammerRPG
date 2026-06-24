using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    internal class TagsPopupWindow : PopupWindowContent
    {
        public static void Show(
            Rect screenRect,
            IEnumerable<string> selectedTags,
            IEnumerable<string> allTags,
            Action<List<string>> onApply,
            Action<string> onAddNewTag)
        {
            var wnd = new TagsPopupWindow(selectedTags, allTags, onApply, onAddNewTag);
            PopupWindow.Show(screenRect, wnd);
        }

        private readonly Action<List<string>> _onApply;
        private readonly Action<string> _onAddNewTag;

        private readonly List<string> _workingSelected;
        private readonly List<string> _allTagsList;

        private string _newTagText = "";
        private string _searchText = "";
        private Vector2 _scroll;

        private TagsPopupWindow(
            IEnumerable<string> selectedTags,
            IEnumerable<string> allTags,
            Action<List<string>> onApply,
            Action<string> onAddNewTag)
        {
            _workingSelected = selectedTags?.Distinct().ToList() ?? new List<string>();
            _allTagsList = allTags?.Distinct().OrderBy(s => s).ToList() ?? new List<string>();
            _onApply = onApply;
            _onAddNewTag = onAddNewTag;
        }

        public override Vector2 GetWindowSize() => new Vector2(300, 360);

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("Теги", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                _searchText = EditorGUILayout.TextField(_searchText, GUI.skin.FindStyle("ToolbarSeachTextField") ?? GUI.skin.textField);
                if (GUILayout.Button("Сбросить", GUILayout.Width(80)))
                {
                    _searchText = "";
                    GUI.FocusControl(null);
                }
            }

            GUILayout.Space(4);

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                var list = FilteredTags();

                if (list.Count == 0)
                {
                    GUILayout.Label(string.IsNullOrWhiteSpace(_searchText)
                        ? "Пока нет доступных тегов."
                        : "Ничего не найдено.");
                }
                else
                {
                    _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(140), GUILayout.ExpandHeight(true));
                    foreach (var tag in list)
                    {
                        var has = _workingSelected.Contains(tag);
                        var toggle = EditorGUILayout.ToggleLeft(tag, has);
                        if (toggle != has)
                        {
                            if (toggle) _workingSelected.Add(tag);
                            else _workingSelected.Remove(tag);
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
            }

            GUILayout.Space(6);

            GUILayout.Label("Добавить новый тег:", EditorStyles.label);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.SetNextControlName("NewTagField");
                _newTagText = EditorGUILayout.TextField(_newTagText);

                var canAdd = !string.IsNullOrWhiteSpace((_newTagText ?? "").Trim());
                using (new EditorGUI.DisabledScope(!canAdd))
                {
                    if (GUILayout.Button("Добавить", GUILayout.Width(90)))
                    {
                        AddNewTagAndSelect();
                    }
                }
            }

            var e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return)
            {
                if (!string.IsNullOrWhiteSpace((_newTagText ?? "").Trim()))
                {
                    AddNewTagAndSelect();
                    e.Use();
                }
            }

            GUILayout.FlexibleSpace();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Отмена"))
                {
                    editorWindow.Close();
                }
                if (GUILayout.Button("Применить"))
                {
                    var result = _workingSelected
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => s.Trim())
                        .Distinct()
                        .OrderBy(s => s)
                        .ToList();

                    _onApply?.Invoke(result);
                    editorWindow.Close();
                }
            }
        }

        private void AddNewTagAndSelect()
        {
            var trimmed = (_newTagText ?? "").Trim();
            if (string.IsNullOrEmpty(trimmed)) return;

            if (!_workingSelected.Contains(trimmed))
                _workingSelected.Add(trimmed);

            _onAddNewTag?.Invoke(trimmed);

            if (!_allTagsList.Contains(trimmed))
                _allTagsList.Add(trimmed);
            _allTagsList.Sort(StringComparer.Ordinal);

            _newTagText = "";
            GUI.FocusControl("NewTagField");
        }

        private List<string> FilteredTags()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
                return _allTagsList;

            var q = _searchText.Trim();
            return _allTagsList.Where(t => t.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }
    }
}