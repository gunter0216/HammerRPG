using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.ModuleItem.Runtime.Config;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace App.Common.ModuleItem.Editor
{
    [CustomEditor(typeof(ModuleItemGameConfig)), CanEditMultipleObjects]
    public class ModuleItemGameConfigEditor : UnityEditor.Editor
    {
        private static readonly List<Type> s_ModuleTypes = new();

        private SerializedProperty _modulesProperty;

        private void OnEnable()
        {
            CollectModuleTypes();

            _modulesProperty = serializedObject.FindProperty("m_Modules");
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

            for (int i = 0; i < _modulesProperty.arraySize; i++)
            {
                var moduleProp = _modulesProperty.GetArrayElementAtIndex(i);
                string moduleTypeName = GetManagedReferenceTypeName(moduleProp);

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                    {
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.LabelField($"{i + 1}. {moduleTypeName}", EditorStyles.boldLabel);
                        GUILayout.FlexibleSpace();

                        var originalColor = GUI.color;
                        GUI.color = new Color(1f, 0.4f, 0.4f, 1f);
                        var buttonStyle = new GUIStyle(GUI.skin.button)
                        {
                            fontStyle = FontStyle.Bold
                        };
                        buttonStyle.normal.textColor = Color.white;

                        if (GUILayout.Button("×", buttonStyle, GUILayout.Width(25), GUILayout.Height(18)))
                        {
                            GUI.color = originalColor;
                            RemoveModuleAt(i);
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();
                            break;
                        }
                        GUI.color = originalColor;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(1);

                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(moduleProp, GUIContent.none, true);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(1);
            }
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

        public ModuleTypeDropdownItem(Type moduleType) : base(moduleType.Name)
        {
            ModuleType = moduleType;
        }
    }
}