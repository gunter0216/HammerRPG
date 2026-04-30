using System.IO;
using App.Common.Data.Runtime;
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

        [MenuItem("Helper/Data/Clear", false, 2)]
        public static void ClearData()
        {
            if (Directory.Exists(Application.persistentDataPath))
            {
                Directory.Delete(Application.persistentDataPath, true);
            }
        }
        
        [MenuItem("Helper/Data/Save", false, 3)]
        public static void SaveData()
        {
            // var serviceProvider = DiManager.Instance.GetCurrentServiceProvider();
            // var dataManager = serviceProvider.GetService<IDataManager>();
            // dataManager.SaveProgress();
        }
#endif
    }
}