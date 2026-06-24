using UnityEditor;
using UnityEngine;

namespace App.Common.Configs.Editor
{
    public class ConfigsStyle
    {
        private GUIStyle _selectedLabelStyle;
        private GUIStyle _normalLabelStyle;
        private bool     _initialized;

        public void OnGUI() => InitStyles();

        private void InitStyles()
        {
            if (_initialized) return;

            _normalLabelStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft,
                padding   = new RectOffset(4, 4, 0, 0),
            };

            _selectedLabelStyle = new GUIStyle(_normalLabelStyle)
            {
                fontStyle = FontStyle.Bold,
            };
            _selectedLabelStyle.normal.textColor = Color.white;

            _initialized = true;
        }

        public GUIStyle SelectedLabelStyle => _selectedLabelStyle;
        public GUIStyle NormalLabelStyle   => _normalLabelStyle;

        // kept for any external code that still references these
        public GUIStyle SelectedButtonStyle => _selectedLabelStyle;
        public GUIStyle NormalButtonStyle   => _normalLabelStyle;
    }
}
