using App.Game.Dungeon.DungeonCore.External.View;
using UnityEditor;

namespace App.Game.Dungeon.DungeonCore.Editor
{
    [CustomEditor(typeof(CreatingModuleItemView)), CanEditMultipleObjects]
    public sealed class CreatingModuleItemViewEditor : UnityEditor.Editor
    {
        private SerializedProperty _script;
        private SerializedProperty _objectId;
        private SerializedProperty _config;

        private void OnEnable()
        {
            _script = serializedObject.FindProperty("m_Script");
            _objectId = serializedObject.FindProperty("_objectId");
            _config = serializedObject.FindProperty("_config");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_script);
                EditorGUILayout.PropertyField(_objectId);
            }

            EditorGUILayout.PropertyField(_config);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
