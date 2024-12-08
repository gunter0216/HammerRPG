using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.AssemblyManager.Runtime;
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
            
        }

        public void Init(List<AttributeNode> singletons, List<AttributeNode> scopeds, List<AttributeNode> configurators)
        {
            for (int i = 0; i < singletons.Count; ++i)
            {
                m_ServiceCollection.AddSingleton(singletons[i].Holder);
            }
            
            for (int i = 0; i < scopeds.Count; ++i)
            {
                var scoped = scopeds[i].Attribute as ScopedAttribute;
                m_ServiceCollection.AddScoped(scopeds[i].Holder, scoped.Context);
            }
            
            for (int i = 0; i < configurators.Count; ++i)
            {
                m_ServiceCollection.AddConfigurator(configurators[i].Holder);
            }
            
            m_ServiceCollection.PreBuild();
        }

        public IServiceProvider BuildServiceProvider(object context)
        {
            var sceneScopeds = GetScopedFromCurrentScene();
            return m_ServiceCollection.BuildServiceProvider(context, sceneScopeds);
        }
        
        private List<object> GetScopedFromCurrentScene()
        {
            var monoScopedFromSceneExtractor = new MonoScopedFromSceneExtractor();
            return monoScopedFromSceneExtractor.GetScopedFromCurrentScene();
        }
    }
}