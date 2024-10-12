using UnityEditor;
using UnityEngine;

namespace App.Common.Editor
{
    public class HelperEditor
    {
#if UNITY_EDITOR
        [MenuItem("Helper/ClearPlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
#endif
}