using System.Collections.Generic;
using System.Reflection;
using App.Common.AssemblyManager.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Collection;
using IServiceProvider = App.Common.Autumn.Runtime.Provider.IServiceProvider;

namespace App.Common.Autumn.External
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

        public void Init(
            IReadOnlyList<AttributeNode> singletons, 
            IReadOnlyList<AttributeNode> scopeds, 
            IReadOnlyList<AttributeNode> transients,
            IReadOnlyList<AttributeNode> configurators)
        {
            for (int i = 0; i < singletons.Count; ++i)
            {
                m_ServiceCollection.AddSingleton(singletons[i].Holder);
            }
            
            for (int i = 0; i < transients.Count; ++i)
            {
                m_ServiceCollection.AddTransient(transients[i].Holder);
            }
            
            for (int i = 0; i < scopeds.Count; ++i)
            {
                var scoped = scopeds[i].Holder.GetCustomAttributes<ScopedAttribute>();
                foreach (var attribute in scoped)
                {
                    m_ServiceCollection.AddScoped(scopeds[i].Holder, attribute.Context);
                }
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