using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    internal class SearchAndFilterPanel<T> : VisualElement where T : ScriptableObject
    {
        private TextField _searchField;
        private Button _tagsButton;

        public Action<string> OnSearchChanged { get; set; }
        public Action<HashSet<string>> OnTagsChanged { get; set; }

        public List<string> AllTags => _allTags;

        private List<string> _allTags = new();
        private readonly HashSet<string> _selectedTags = new();

        private string _pendingSearchText;
        private bool _isDebounceScheduled;

        public SearchAndFilterPanel()
        {
            AddToClassList("search-and-filter-panel");

            _searchField = new TextField()
            {
                style =
                {
                    flexGrow = 1,
                }
            };
            _searchField.RegisterValueChangedCallback(evt =>
            {
                _pendingSearchText = evt.newValue;
                ScheduleDebounce();
            });

            Add(_searchField);

            _tagsButton = new Button(OpenTagsPopup)
            {
                iconImage = Background.FromTexture2D((Texture2D)EditorGUIUtility.IconContent("d_FilterByLabel@2x").image),
                style =
                {
                    marginLeft = 5,
                    flexGrow = 0,
                    flexShrink = 0
                }
            };
            Add(_tagsButton);
        }

        public void RefreshAvailableTags(IEnumerable<T> configs)
        {
            var db = ScriptableObjectTagsDatabase.Instance;
            if (db == null)
            {
                _allTags = new List<string>();
                return;
            }

            var soList = configs?.Cast<ScriptableObject>() ?? Enumerable.Empty<ScriptableObject>();
            _allTags = db.GetAllTagsFromObjects(soList);
        }

        public IReadOnlyList<string> GetAvailableTags() => _allTags;

        public void AddNewTags(List<string> tags)
        {
            var tagSet = new HashSet<string>();
            foreach (var tag in tags)
            {
                if (!string.IsNullOrWhiteSpace(tag) && !_allTags.Contains(tag.Trim()))
                    tagSet.Add(tag.Trim());
            }

            _allTags = tagSet.OrderBy(t => t).ToList();
            _selectedTags.IntersectWith(_allTags);
        }

        private void ScheduleDebounce()
        {
            if (_isDebounceScheduled) return;
            _isDebounceScheduled = true;
            EditorApplication.delayCall += DebounceCallback;
        }

        private void DebounceCallback()
        {
            EditorApplication.delayCall -= DebounceCallback;
            _isDebounceScheduled = false;
            OnSearchChanged?.Invoke(_pendingSearchText);
        }

        private void OpenTagsPopup()
        {
            var rect = _tagsButton.worldBound;
            var screenPos = rect.position;
            var popupRect = new Rect(screenPos, new Vector2(0, 0));
            
            var selected = new List<string>(_selectedTags);
            var all = _allTags ?? new List<string>();

            FilterTagsPopupWindow.Show(
                popupRect,
                selected,
                all,
                applied =>
                {
                    _selectedTags.Clear();
                    foreach (var t in applied)
                        _selectedTags.Add(t);

                    OnTagsChanged?.Invoke(new HashSet<string>(_selectedTags));
                }
            );
        }

        public HashSet<string> GetSelectedTags() => new HashSet<string>(_selectedTags);
        public string CurrentSearchText => _searchField?.value ?? "";
    }

    internal class FilterTagsPopupWindow : PopupWindowContent
    {
        public static void Show(
            Rect screenRect,
            IEnumerable<string> selectedTags,
            IEnumerable<string> allTags,
            Action<List<string>> onApply)
        {
            var wnd = new FilterTagsPopupWindow(selectedTags, allTags, onApply);
            UnityEditor.PopupWindow.Show(screenRect, wnd);
        }

        private readonly Action<List<string>> _onApply;
        private readonly List<string> _workingSelected;
        private readonly List<string> _allTagsList;

        private string _searchText = "";
        private Vector2 _scroll;

        private FilterTagsPopupWindow(
            IEnumerable<string> selectedTags,
            IEnumerable<string> allTags,
            Action<List<string>> onApply)
        {
            _workingSelected = selectedTags?.Distinct().ToList() ?? new List<string>();
            _allTagsList = allTags?.Distinct().OrderBy(s => s).ToList() ?? new List<string>();
            _onApply = onApply;
        }

        public override Vector2 GetWindowSize() => new Vector2(300, 360);

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("Фильтр по тегам", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                _searchText = EditorGUILayout.TextField(
                    _searchText,
                    GUI.skin.FindStyle("ToolbarSeachTextField") ?? GUI.skin.textField
                );

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
                    _scroll = EditorGUILayout.BeginScrollView(
                        _scroll, GUILayout.MinHeight(140), GUILayout.ExpandHeight(true));

                    foreach (var tag in list)
                    {
                        bool has = _workingSelected.Contains(tag);
                        bool toggle = EditorGUILayout.ToggleLeft(tag, has);
                        if (toggle != has)
                        {
                            if (toggle) _workingSelected.Add(tag);
                            else _workingSelected.Remove(tag);
                        }
                    }

                    EditorGUILayout.EndScrollView();
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

        private List<string> FilteredTags()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
                return _allTagsList;

            var q = _searchText.Trim();
            return _allTagsList
                .Where(t => t.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
    }
}