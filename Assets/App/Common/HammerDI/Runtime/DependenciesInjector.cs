﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.HammerDI.Runtime.Attributes;
using UnityEngine;

namespace App.Common.HammerDI.Runtime
{
    public class DependenciesInjector
    {
        public void InjectDependencies(
            Dictionary<Type, object> services,
            Dictionary<Type, List<object>> interfaces,
            Dictionary<Type, Func<Type>> transients)
        {
            foreach (var service in services)
            {
                var serviceType = service.Key;
                var fields = serviceType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];
                    var injectAttribute = field.GetCustomAttribute<InjectAttribute>();
                    if (injectAttribute == null)
                    {
                        continue;
                    }

                    var fieldType = field.FieldType;
                    object instance; 
                    if (fieldType.IsInterface)
                    {
                        if (!interfaces.TryGetValue(fieldType, out var instanceList))
                        {
                            throw new ArgumentException($"Cant inject {fieldType} in {serviceType.Name}");
                        }

                        instance = instanceList.First();
                    }
                    else if (FieldIsListInterfaces(fieldType))
                    {
                        var genericType = fieldType.GenericTypeArguments[0];
                        if (!interfaces.TryGetValue(genericType, out var instanceList))
                        {
                            throw new ArgumentException($"Cant inject {genericType} in {serviceType.Name} instanceList not found.");
                        }
                        
                        var listType = typeof(List<>).MakeGenericType(genericType);
                        var list = (IList)Activator.CreateInstance(listType);
                        foreach (var value in instanceList)
                        {
                            list.Add(value);
                        }

                        instance = list;
                    }
                    else
                    {
                        if (!services.TryGetValue(field.FieldType, out instance))
                        {
                            throw new ArgumentException($"Cant inject {fieldType} in {serviceType.Name}");
                        }
                    }
                    
                    field.SetValue(service.Value, instance);
                }
            }
        }

        private bool FieldIsListInterfaces(Type fieldType)
        {
            return fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}