using System.Collections.Generic;
using System.Reflection;
using App.Common.HammerDI.Runtime.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Common.HammerDI.External
{
    public class MonoScopedFromSceneExtractor
    {
        internal List<object> GetScopedFromCurrentScene()
        {
            var allSceneObjects = new List<MonoBehaviour>();
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                var monoBehaviours = rootGameObject.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);
                allSceneObjects.AddRange(monoBehaviours);
            }

            var monoScopeds = new List<object>();
            foreach (var sceneObject in allSceneObjects)
            {
                var monoType = sceneObject.GetType();
                var monoScoped = monoType.GetCustomAttribute<MonoScopedAttribute>();
                if (monoScoped != null)
                {
                    monoScopeds.Add(sceneObject);
                }
            }

            return monoScopeds;
        }
    }
}