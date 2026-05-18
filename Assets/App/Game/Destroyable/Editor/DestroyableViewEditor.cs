using App.Game.Destroyable.External;
using UnityEditor;
using UnityEngine;

namespace App.Game.Destroyable.Editor
{
    [CustomEditor(typeof(DestroyableView))]
    public class DestroyableViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var view = (DestroyableView)target;

            if (GUILayout.Button("Destroy"))
            {
                view.Destroy();
            }
        }
    }
}