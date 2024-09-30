using System.Reflection;
using App.Common.HammerDI.Runtime;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Interfaces;

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
            return m_ServiceCollection.BuildServiceProvider(context);
        }
    }
}