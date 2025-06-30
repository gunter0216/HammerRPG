using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Provider;
using App.Common.Autumn.Runtime.Services;
using UnityEngine;
using IServiceProvider = App.Common.Autumn.Runtime.Provider.IServiceProvider;

namespace App.Common.Autumn.Runtime.Collection
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly Dictionary<RuntimeTypeHandle, Func<object>> m_TransientsFabrics = new();

        private readonly Dictionary<object, HashSet<RuntimeTypeHandle>> m_Contexts = new();
        private readonly Dictionary<RuntimeTypeHandle, object> m_Singletons = new();
        private readonly HashSet<RuntimeTypeHandle> m_Transients = new();

        private readonly InterfacesExtractor m_InterfacesExtractor;
        private readonly DependenciesInjector m_DependenciesInjector;
        private readonly ConfigurationCollection m_ConfigurationCollection;

        public ServiceCollection()
        {
            m_DependenciesInjector = new DependenciesInjector();
            m_InterfacesExtractor = new InterfacesExtractor();
            m_ConfigurationCollection = new ConfigurationCollection(this);
        }

        public void PreBuild()
        {
            InjectDependenciesIntoSingletons();
        }

        private void InjectDependenciesIntoSingletons()
        {
            var values = m_Singletons.Values.ToArray();
            var interfaces = m_InterfacesExtractor.CreateInterfaceToInstances(values);
            m_DependenciesInjector.InjectDependencies(m_Singletons, interfaces, m_TransientsFabrics);

            foreach (var singletonObj in m_Singletons.Values)
            {
                if (singletonObj is ISingleton singleton)
                {
                    singleton.OnInjectEnd();
                }
            }
        }

        public IServiceProvider BuildServiceProvider(object context, List<object> additional)
        {
            if (!m_Contexts.TryGetValue(context, out var scopedsFromContext))
            {
                throw new ArgumentException($"Context not found {context}");
            }

            int scopedsAmount = scopedsFromContext.Count + additional.Count;
            int servicesAmount = scopedsAmount + m_Singletons.Count;
            
            var allScopeds = new Dictionary<RuntimeTypeHandle, object>(scopedsAmount);
            var allServices = new Dictionary<RuntimeTypeHandle, object>(servicesAmount);
            foreach (var scoped in scopedsFromContext)
            {
                var type = Type.GetTypeFromHandle(scoped);
                var instance = Activator.CreateInstance(type);
                allScopeds.Add(type.TypeHandle, instance);
                allServices.Add(type.TypeHandle, instance);
            }

            foreach (var obj in additional)
            {
                var type = obj.GetType();
                allScopeds.Add(type.TypeHandle, obj);
                allServices.Add(type.TypeHandle, obj);
            }
            
            foreach (var singleton in m_Singletons)
            {
                var type = Type.GetTypeFromHandle(singleton.Key); 
                allServices.Add(type.TypeHandle, singleton.Value);
            }

            var servicesList = allServices.Values.ToArray();
            var interfaces = m_InterfacesExtractor.CreateInterfaceToInstances(servicesList);
            m_DependenciesInjector.InjectDependencies(allScopeds, interfaces, m_TransientsFabrics);
            
            return new ServiceProvider(allServices, interfaces);
        }

        public void AddTransient(Type type)
        {
            if (!m_Transients.Add(type.TypeHandle))
            {
                throw new ArgumentException($"Cant add transient, key is already exists {type.FullName}");
            }
        }

        public void AddScoped(Type type, object context)
        {
            if (!m_Contexts.TryGetValue(context, out var scopeds))
            {
                scopeds = new HashSet<RuntimeTypeHandle>();
                m_Contexts.Add(context, scopeds);
            }
            
            scopeds.Add(type.TypeHandle);
        }

        public void AddSingleton(Type type)
        {
            var handle = type.TypeHandle;
            if (m_Singletons.ContainsKey(handle))
            {
                throw new ArgumentException($"Cant add singleton, key is already exists {type.FullName}");
            }
            
            var instance = Activator.CreateInstance(type);
            m_Singletons.Add(handle, instance);
        }

        private void AddSingleton(Type type, object instance)
        {
            var handle = type.TypeHandle;
            if (m_Singletons.ContainsKey(handle))
            {
                throw new ArgumentException($"Cant add singleton, key is already exists {type.FullName}");
            }
            
            m_Singletons.Add(handle, instance);
        }

        public void AddConfigurator(Type configuratorType)
        {
            var configuratorInstance = Activator.CreateInstance(configuratorType);
            foreach (var method in configuratorType.GetMethods())
            {
                if (HasAttribute(method, typeof(SingletonAttribute)))
                {
                    if (method.ReturnType != typeof(void))
                    {
                        AddSingleton(method.ReturnType, method.Invoke(configuratorInstance, parameters: null));
                    }
                } 
            }
            
            if (configuratorInstance is IConfigurator configurator)
            {
                configurator.Configuration(m_ConfigurationCollection);
            }
        }
        
        private bool HasAttribute(MethodInfo method, Type attribute)
        {
            return method.IsDefined(attribute, false);
        }
        
        public void UnloadContext(object context)
        {
            m_Contexts.Remove(context);
        }
    }
}