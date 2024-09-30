using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace App.Common.Scenes.Editor
{
    public class SceneEditor : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("Helper/Scenes/StartScene")]
        public static void GoToStartScene()
        {
            OpenScene("Assets/Scenes/StartScene.unity");
        }
        
        [MenuItem("Helper/Scenes/MenuScene")]
        public static void GoToMetaScene()
        {
            OpenScene("Assets/Scenes/MenuScene.unity");
        }
        
        [MenuItem("Helper/Scenes/GameScene")]
        public static void GoToCoreScene()
        {
            OpenScene("Assets/Scenes/GameScene.unity");
        }

        private static void OpenScene(string name)
        {
            if (Application.isPlaying)
            {
                Debug.Log("Open scene only in Edit mode!");
                return;
            }
            
            bool isSaved = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), EditorSceneManager.GetActiveScene().path);
            Debug.Log("Saved Scene " + (isSaved ? "OK" : "Error!"));
            EditorSceneManager.OpenScene(name);
        }
#endif
    }
}