using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Interfaces;
using App.Common.Logger.Runtime;
using UnityEngine;
using IServiceProvider = App.Common.HammerDI.Runtime.Interfaces.IServiceProvider;

namespace App.Common.HammerDI.Runtime
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly Dictionary<Type, object> m_Transients = new();
        private readonly Dictionary<object, Dictionary<Type, Type>> m_Contexts = new();
        private readonly Dictionary<Type, object> m_Singletons = new();

        private readonly InterfacesExtractor m_InterfacesExtractor = new();
        private readonly DependenciesInjector m_DependenciesInjector = new();
        
        public IServiceProvider BuildServiceProvider(object context)
        {
            if (!m_Contexts.TryGetValue(context, out var scopeds))
            {
                throw new ArgumentException($"Cant get context {context}");
            }
            
            var services = new Dictionary<Type, object>(scopeds.Count + m_Singletons.Count);
            foreach (var scoped in scopeds)
            {
                var instance = Activator.CreateInstance(scoped.Key);
                services.Add(scoped.Key, instance);
            }
            
            foreach (var singleton in m_Singletons)
            {
                services.Add(singleton.Key, singleton.Value);
            }

            var interfaces = m_InterfacesExtractor.ExtractInterfaces(services);
            m_DependenciesInjector.InjectDependencies(services, interfaces);
            
            return new ServiceProvider(services, interfaces);
        }

        public void AddTransient(Type type)
        {
            throw new NotImplementedException();
        }

        public void AddScoped(Type type, object context)
        {
            if (!m_Contexts.TryGetValue(context, out var scopeds))
            {
                scopeds = new Dictionary<Type, Type>();
                m_Contexts.Add(context, scopeds);
            }
            
            // var instance = Activator.CreateInstance(type);
            scopeds.Add(type, type);
        }

        public void AddSingleton(Type type)
        {
            if (m_Singletons.ContainsKey(type))
            {
                throw new ArgumentException($"Cant add singleton, key is already exists {type.Name}");
            }
            
            var instance = Activator.CreateInstance(type);
            m_Singletons.Add(type, instance);
        }

        public void UnloadContext(object context)
        {
            m_Contexts.Remove(context);
        }
    }
}