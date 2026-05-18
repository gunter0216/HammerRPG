using App.Common.SceneControllers.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Common.SceneControllers.Editor
{
    public class SceneEditor
    {
#if UNITY_EDITOR
        [MenuItem("Helper/Scenes/Start", false, 1)]
        public static void GoToStartScene()
        {
            OpenScene($"Assets/Scenes/{SceneConstants.StartScene}.unity");
        }
        
        [MenuItem("Helper/Scenes/Menu", false, 2)]
        public static void GoToMenuScene()
        {
            OpenScene($"Assets/Scenes/{SceneConstants.MenuScene}.unity");
        }
        
        [MenuItem("Helper/Scenes/Core", false, 3)]
        public static void GoToCoreScene()
        {
            OpenScene($"Assets/Scenes/{SceneConstants.CoreScene}.unity");
        }
        
        [MenuItem("Helper/Scenes/DungeonTest", false, 4)]
        public static void GoToDungeonTestScene()
        {
            OpenScene($"Assets/Scenes/{SceneConstants.DungeonTest}.unity");
        }

        private static void OpenScene(string name)
        {
            if (Application.isPlaying)
            {
                Debug.Log("Open scene only in Edit mode!");
                return;
            }
            
            EditorSceneManager.OpenScene(name);
        }
#endif
    }
}