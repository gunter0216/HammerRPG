﻿using System;
using System.Collections.Generic;

namespace App.Common.Autumn.Runtime.Provider
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly Dictionary<RuntimeTypeHandle, object> m_Services;
        private readonly Dictionary<RuntimeTypeHandle, List<object>> m_Interfaces;

        public ServiceProvider(
            Dictionary<RuntimeTypeHandle, object> services, 
            Dictionary<RuntimeTypeHandle, List<object>> interfaces)
        {
            m_Services = services;
            m_Interfaces = interfaces;
        }
        
        public T GetService<T>() where T : class
        {
            if (m_Services.TryGetValue(typeof(T).TypeHandle, out var instance))
            {
                return instance as T;
            }

            return null;
        }
        
        public List<object> GetInterfaces<T>() where T : class
        {
            if (m_Interfaces.TryGetValue(typeof(T).TypeHandle, out var objects))
            {
                return objects;
            }

            return new List<object>();
        }
    }
}