using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Interfaces;
using UnityEngine;
using IServiceProvider = App.Common.HammerDI.Runtime.Interfaces.IServiceProvider;

namespace App.Common.HammerDI.Runtime
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly Dictionary<Type, Func<Type>> m_Transients = new();
        private readonly Dictionary<object, Dictionary<Type, Type>> m_Contexts = new();
        private readonly Dictionary<Type, object> m_Singletons = new();

        private readonly InterfacesExtractor m_InterfacesExtractor = new();
        private readonly DependenciesInjector m_DependenciesInjector = new();

        public void PreBuild()
        {
            InjectDependenciesIntoSingletons();
        }

        private void InjectDependenciesIntoSingletons()
        {
            var interfaces = m_InterfacesExtractor.ExtractInterfaces(m_Singletons);
            m_DependenciesInjector.InjectDependencies(m_Singletons, interfaces, m_Transients);
        }
        
        public void AddConfigurator(Type configurator)
        {
            var configuratorInstance = Activator.CreateInstance(configurator);
            foreach (var method in configurator.GetMethods())
            {
                var singleton = method.GetCustomAttribute<SingletonAttribute>();
                if (singleton != null)
                {
                    if (method.ReturnType != typeof(void))
                    {
                        AddSingleton(method.ReturnType, method.Invoke(configuratorInstance, parameters: null));
                    }
                }
            }
        }
        
        public IServiceProvider BuildServiceProvider(object context, List<object> sceneScopeds)
        {
            if (!m_Contexts.TryGetValue(context, out var scopedsFromContext))
            {
                throw new ArgumentException($"Cant get context {context}");
            }

            int scopedsAmount = scopedsFromContext.Count + sceneScopeds.Count;
            int servicesAmount = scopedsAmount + m_Singletons.Count;

            var allServices = new Dictionary<Type, object>(servicesAmount);
            var allScopeds = new Dictionary<Type, object>(scopedsAmount);
            foreach (var scoped in scopedsFromContext)
            {
                var instance = Activator.CreateInstance(scoped.Key);
                allServices.Add(scoped.Key, instance);
                allScopeds.Add(scoped.Key, instance);
            }

            foreach (var scoped in sceneScopeds)
            {
                var type = scoped.GetType();
                allServices.Add(type, scoped);
                allScopeds.Add(type, scoped);
            }

            foreach (var singleton in m_Singletons)
            {
                allServices.Add(singleton.Key, singleton.Value);
            }

            var interfaces = m_InterfacesExtractor.ExtractInterfaces(allServices);
            m_DependenciesInjector.InjectDependencies(allScopeds, interfaces, m_Transients);
            
            return new ServiceProvider(allServices, interfaces);
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
        
        public void AddSingleton(Type type, object instance)
        {
            if (m_Singletons.ContainsKey(type))
            {
                throw new ArgumentException($"Cant add singleton, key is already exists {type.Name}");
            }
            
            m_Singletons.Add(type, instance);
        }

        public void UnloadContext(object context)
        {
            m_Contexts.Remove(context);
        }
    }
}