using System.IO;
using UnityEditor;
using UnityEngine;

namespace App.Common.Data.Editor
{
    public class DataEditor
    {
#if UNITY_EDITOR
        [MenuItem("Helper/Data/OpenDataFolder", false, 1)]
        public static void GoToStartScene()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("Helper/ClearData")]
        public static void ClearData()
        {
            if (Directory.Exists(Application.persistentDataPath))
            {
                Directory.Delete(Application.persistentDataPath, true);
            }
        }
#endif
    }
}