// using App.Game.Player.External.Animations;
// using UnityEditor;
// using UnityEngine;
//
// namespace App.Game.Player.Editor
// {
//     [CustomEditor(typeof(EntityMeleeWeaponAnimation))]
//     public class EntityMeleeWeaponAnimationEditor : UnityEditor.Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             DrawDefaultInspector();
//             var myScript = (EntityMeleeWeaponAnimation)target;
//             if (GUILayout.Button("Open File"))
//             {
//                 myScript.Play();
//             }
//         }
//     }
// }