using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.ModuleItem.Runtime.Config;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEditorInternal;

namespace App.Common.ModuleItem.Editor
{
    [CustomEditor(typeof(ModuleItemGameConfig)), CanEditMultipleObjects]
    public class ModuleItemGameConfigEditor : UnityEditor.Editor
    {
        private static GUIStyle s_BoldFoldoutStyle;

        private static GUIStyle BoldFoldoutStyle =>
            s_BoldFoldoutStyle ??= new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold
            };

        private static readonly List<Type> s_ModuleTypes = new();

        private SerializedProperty _modulesProperty;
        private ReorderableList _modulesList;

        private void OnEnable()
        {
            CollectModuleTypes();

            _modulesProperty = serializedObject.FindProperty("m_Modules");

            _modulesList = new ReorderableList(
                serializedObject,
                _modulesProperty,
                draggable: true,
                displayHeader: false,
                displayAddButton: false,
                displayRemoveButton: false)
            {
                drawElementCallback = DrawModuleElement,
                elementHeightCallback = GetModuleElementHeight,
                // немного отступа сверху/снизу под нашу собственную "box"-рамку
                footerHeight = 0
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Рисуем все обычные поля (m_Id, m_Type, m_Tags), кроме списка модулей —
            // его отрисуем отдельно, с кастомными кнопками add/remove.
            DrawPropertiesExcluding(serializedObject, "m_Modules");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Modules", EditorStyles.boldLabel);

            DrawModulesList();

            serializedObject.ApplyModifiedProperties();

            DrawAddModuleButton();
        }

        private void DrawModulesList()
        {
            if (_modulesProperty == null || !_modulesProperty.isArray)
            {
                EditorGUILayout.HelpBox("Свойство m_Modules не найдено.", MessageType.Warning);
                return;
            }

            _modulesList.DoLayoutList();
        }

        private float GetModuleElementHeight(int index)
        {
            var moduleProp = _modulesProperty.GetArrayElementAtIndex(index);

            float height = EditorGUIUtility.singleLineHeight + 4f; // строка заголовка
            height += 4f; // верхний/нижний паддинг box'а

            if (moduleProp.isExpanded)
            {
                height += GetChildrenHeight(moduleProp) + 4f;
            }

            return height + 4f;
        }

        private static float GetChildrenHeight(SerializedProperty property)
        {
            float height = 0f;
            var child = property.Copy();
            var end = property.GetEndProperty();
            bool enterChildren = true;

            while (child.NextVisible(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                height += EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;
                enterChildren = false;
            }

            return height;
        }

        private const float DragHandleWidth = 15f;

        private void DrawModuleElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var moduleProp = _modulesProperty.GetArrayElementAtIndex(index);
            string moduleTypeName = GetManagedReferenceTypeName(moduleProp);
            moduleTypeName = moduleTypeName.Replace("ModuleConfig", string.Empty);

            rect.y += 2f;
            rect.height -= 4f;

            // Один box на всю ширину строки (включая зону под системную ручку) —
            // перекрывает стандартную "плавающую" иконку ReorderableList,
            // которая иначе центрируется по всей (переменной) высоте элемента.
            GUI.Box(rect, GUIContent.none, "box");

            var contentRect = new Rect(rect.x + DragHandleWidth, rect.y, rect.width - DragHandleWidth, rect.height);

            var removeButtonRect = new Rect(contentRect.xMax - 27, contentRect.y + 2, 25, 18);
            var foldoutRect = new Rect(contentRect.x + 4, contentRect.y + 2,
                contentRect.width - 8 - removeButtonRect.width, EditorGUIUtility.singleLineHeight);

            moduleProp.isExpanded = EditorGUI.Foldout(
                foldoutRect,
                moduleProp.isExpanded,
                $"{index + 1}. {moduleTypeName}",
                true,
                BoldFoldoutStyle);

            var originalColor = GUI.color;
            GUI.color = new Color(1f, 0.4f, 0.4f, 1f);
            var buttonStyle = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold };
            buttonStyle.normal.textColor = Color.white;

            if (GUI.Button(removeButtonRect, "×", buttonStyle))
            {
                GUI.color = originalColor;
                int indexToRemove = index;
                EditorApplication.delayCall += () => RemoveModuleAt(indexToRemove);
                return;
            }

            GUI.color = originalColor;

            if (moduleProp.isExpanded)
            {
                var childContentRect = new Rect(
                    contentRect.x + 4,
                    foldoutRect.yMax + 4,
                    contentRect.width - 8,
                    contentRect.height - foldoutRect.height - 8);

                DrawChildren(childContentRect, moduleProp);
            }
        }

        private static void DrawChildren(Rect rect, SerializedProperty property)
        {
            var child = property.Copy();
            var end = property.GetEndProperty();
            bool enterChildren = true;

            float y = rect.y;

            EditorGUI.indentLevel++;
            while (child.NextVisible(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                float height = EditorGUI.GetPropertyHeight(child, true);
                var childRect = new Rect(rect.x, y, rect.width, height);
                EditorGUI.PropertyField(childRect, child, true);
                y += height + EditorGUIUtility.standardVerticalSpacing;
                enterChildren = false;
            }

            EditorGUI.indentLevel--;
        }

        private void RemoveModuleAt(int index)
        {
            serializedObject.ApplyModifiedProperties();

            foreach (var t in targets)
            {
                if (t is ModuleItemGameConfig config)
                {
                    Undo.RecordObject(config, "Remove Module");
                    config.RemoveModuleAt(index);
                    EditorUtility.SetDirty(config);
                }
            }

            serializedObject.Update();
            Repaint();
        }

        private void DrawAddModuleButton()
        {
            EditorGUILayout.BeginHorizontal("box");

            if (s_ModuleTypes.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "Не найдено ни одного класса, наследующего ModuleConfig.",
                    MessageType.Warning);
                EditorGUILayout.EndHorizontal();
                return;
            }

            if (GUILayout.Button("Add Module", GUILayout.Height(30)))
            {
                Rect rect = GUILayoutUtility.GetRect(new GUIContent("Add Module"), EditorStyles.toolbarButton);
                var dropdown = new ModuleTypesDropDown(new AdvancedDropdownState());

                dropdown.Init(s_ModuleTypes, selectedType =>
                {
                    var module = CreateModuleInstance(selectedType);

                    foreach (var t in targets)
                    {
                        if (t is ModuleItemGameConfig config)
                        {
                            Undo.RecordObject(config, "Add Module");
                            config.AddModule(module);
                            EditorUtility.SetDirty(config);
                        }
                    }

                    serializedObject.Update();
                    Repaint();
                });

                dropdown.Show(rect);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static string GetManagedReferenceTypeName(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference ||
                string.IsNullOrEmpty(property.managedReferenceFullTypename))
            {
                return "(missing / broken)";
            }

            var parts = property.managedReferenceFullTypename.Split(' ');
            if (parts.Length != 2 || string.IsNullOrEmpty(parts[1]))
            {
                return "Module";
            }

            var dot = parts[1].LastIndexOf('.');
            return dot >= 0 ? parts[1].Substring(dot + 1) : parts[1];
        }

        private static void CollectModuleTypes()
        {
            s_ModuleTypes.Clear();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types.Where(t => t != null).ToArray();
                }

                foreach (var type in types)
                {
                    if (type.IsAbstract || type.IsInterface) continue;
                    if (type.IsGenericTypeDefinition) continue;
                    if (!typeof(ModuleConfig).IsAssignableFrom(type)) continue;

                    s_ModuleTypes.Add(type);
                }
            }

            s_ModuleTypes.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        }

        private static ModuleConfig CreateModuleInstance(Type type)
        {
            // Часть модулей может не иметь публичного конструктора без параметров
            // (как и сам ModuleItemGameConfig) — для таких создаём "пустой" объект
            // напрямую в памяти, без вызова конструктора. Поля дозаполняются в Inspector.
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                return (ModuleConfig)Activator.CreateInstance(type);
            }

            return (ModuleConfig)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
        }
    }

    class ModuleTypesDropDown : AdvancedDropdown
    {
        private Action<Type> _onTypeSelected;
        private List<Type> _moduleTypes = new();

        public ModuleTypesDropDown(AdvancedDropdownState state) : base(state)
        {
            minimumSize = new Vector2(300, minimumSize.y);
        }

        public void Init(List<Type> moduleTypes, Action<Type> onTypeSelected)
        {
            _moduleTypes = moduleTypes;
            _onTypeSelected = onTypeSelected;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("Modules");

            // id всё ещё нужен Unity для внутреннего учёта (раскрытие/поиск),
            // но для сопоставления с типом полагаемся не на него, а на сам
            // объект Type, который храним в кастомном ModuleTypeDropdownItem.
            for (var index = 0; index < _moduleTypes.Count; index++)
            {
                var item = new ModuleTypeDropdownItem(_moduleTypes[index]) { id = index };
                root.AddChild(item);
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            if (item is ModuleTypeDropdownItem moduleItem)
            {
                _onTypeSelected?.Invoke(moduleItem.ModuleType);
            }
        }
    }

    class ModuleTypeDropdownItem : AdvancedDropdownItem
    {
        public Type ModuleType { get; }

        public ModuleTypeDropdownItem(Type moduleType) : base(GetDisplayName(moduleType))
        {
            ModuleType = moduleType;
        }

        private static string GetDisplayName(Type moduleType)
        {
            return moduleType.Name.Replace("ModuleConfig", string.Empty);
        }
    }
}