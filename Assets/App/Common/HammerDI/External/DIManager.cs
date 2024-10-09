using System;
using System.Collections.Generic;
using System.Reflection;
using App.Common.HammerDI.Runtime;
using App.Common.HammerDI.Runtime.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using IServiceProvider = App.Common.HammerDI.Runtime.Interfaces.IServiceProvider;

namespace App.Common.HammerDI.External
{
    public class DiManager
    {
        private static DiManager m_Instance;
        private readonly ServiceCollection m_ServiceCollection = new();

        public static DiManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new DiManager();
                }
                
                return m_Instance;
            }
        }

        public DiManager()
        {
            var assembly = Assembly.GetCallingAssembly();
            var allTypes = assembly.GetTypes();
            for (int i = 0; i < allTypes.Length; ++i)
            {
                var type = allTypes[i];
                var singleton = type.GetCustomAttribute<SingletonAttribute>(false);
                if (singleton != null)
                {
                    m_ServiceCollection.AddSingleton(type);
                }
                
                var scoped = type.GetCustomAttribute<ScopedAttribute>(false);
                if (scoped != null)
                {
                    m_ServiceCollection.AddScoped(type, scoped.Context);
                }
            }
        }

        public IServiceProvider BuildServiceProvider(object context)
        {
            var sceneScopeds = GetScopedFromCurrentScene();
            return m_ServiceCollection.BuildServiceProvider(context, sceneScopeds);
        }

        private List<object> GetScopedFromCurrentScene()
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