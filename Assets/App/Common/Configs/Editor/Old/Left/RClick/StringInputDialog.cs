using System;
using UnityEditor;
using UnityEngine;

namespace App.Common.Configs.Editor
{
    // ═══════════════════════════════════════════════════════════════════════════
    // Helper: single-field string input dialog (used for Rename & Create Folder)
    // ═══════════════════════════════════════════════════════════════════════════
    internal class StringInputDialog : EditorWindow
    {
        private string        _label;
        private string        _value;
        private Action<string> _onConfirm;
        private bool          _focusSet;

        public static void Show(string title, string label, string defaultValue,
            Action<string> onConfirm)
        {
            var win = CreateInstance<StringInputDialog>();
            win.titleContent = new GUIContent(title);
            win._label       = label;
            win._value       = defaultValue;
            win._onConfirm   = onConfirm;
            win._focusSet    = false;
            win.minSize      = new Vector2(320, 90);
            win.maxSize      = new Vector2(320, 90);
            win.ShowModal();
        }

        private void OnGUI()
        {
            GUILayout.Space(8);
            EditorGUILayout.LabelField(_label);

            GUI.SetNextControlName("InputField");
            _value = EditorGUILayout.TextField(_value);

            if (!_focusSet)
            {
                EditorGUI.FocusTextInControl("InputField");
                _focusSet = true;
            }

            // Confirm on Enter
            if (Event.current.type == EventType.KeyDown &&
                Event.current.keyCode == KeyCode.Return)
            {
                Confirm();
                return;
            }

            GUILayout.Space(6);
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("OK"))     Confirm();
                if (GUILayout.Button("Cancel")) Close();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void Confirm()
        {
            _onConfirm?.Invoke(_value);
            Close();
        }
    }
}