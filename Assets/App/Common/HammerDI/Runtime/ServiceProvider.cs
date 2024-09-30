using System;
using System.Collections.Generic;
using UnityEngine;
using IServiceProvider = App.Common.HammerDI.Runtime.Interfaces.IServiceProvider;

namespace App.Common.HammerDI.Runtime
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> m_Services;
        private readonly Dictionary<Type, List<object>> m_Interfaces;

        public ServiceProvider(Dictionary<Type, object> services, Dictionary<Type, List<object>> interfaces)
        {
            m_Services = services;
            m_Interfaces = interfaces;
        }
        
        public T GetService<T>() where T : class
        {
            if (m_Services.TryGetValue(typeof(T), out var instance))
            {
                return instance as T;
            }

            return null;
        }
        
        public List<object> GetInterfaces<T>() where T : class
        {
            if (m_Interfaces.TryGetValue(typeof(T), out var objects))
            {
                return objects;
            }

            return null;
        }
    }
}