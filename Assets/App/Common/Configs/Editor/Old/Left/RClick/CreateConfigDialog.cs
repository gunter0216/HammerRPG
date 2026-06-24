using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace App.Common.Configs.Editor
{
    // ═══════════════════════════════════════════════════════════════════════════
    // Helper: modal dialog to pick a GameConfig type + enter an asset name
    // ═══════════════════════════════════════════════════════════════════════════
    internal class CreateConfigDialog : EditorWindow
    {
        private List<Type>              _types;
        private string                  _targetFolder;
        private Action<Type, string, string> _onCreate;

        private int    _selectedTypeIndex;
        private string _assetName = "NewConfig";
        private Vector2 _typeScroll;

        public static void Show(List<Type> types, string targetFolder,
            Action<Type, string, string> onCreate)
        {
            var win = CreateInstance<CreateConfigDialog>();
            win.titleContent  = new GUIContent("Create Config");
            win._types        = types;
            win._targetFolder = targetFolder;
            win._onCreate     = onCreate;
            win._assetName    = "NewConfig";
            win.ShowModal();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select type:", EditorStyles.boldLabel);

            _typeScroll = EditorGUILayout.BeginScrollView(_typeScroll,
                GUILayout.Height(Mathf.Min(_types.Count * 20f + 8f, 200f)));
            {
                for (int i = 0; i < _types.Count; i++)
                {
                    bool selected = _selectedTypeIndex == i;
                    if (GUILayout.Toggle(selected, _types[i].Name, "Button") && !selected)
                    {
                        _selectedTypeIndex = i;
                        if (_assetName == "NewConfig" || _types.Any(t => t.Name == _assetName))
                            _assetName = _types[i].Name;
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            GUILayout.Space(6);
            EditorGUILayout.LabelField("Asset name:", EditorStyles.boldLabel);
            _assetName = EditorGUILayout.TextField(_assetName);

            GUILayout.Space(6);
            EditorGUILayout.LabelField($"Folder: {_targetFolder}", EditorStyles.miniLabel);

            GUILayout.Space(8);
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Create"))
                {
                    string name = _assetName.Trim();
                    if (!string.IsNullOrEmpty(name))
                    {
                        _onCreate?.Invoke(_types[_selectedTypeIndex], name, _targetFolder);
                        Close();
                    }
                }
                if (GUILayout.Button("Cancel"))
                    Close();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}