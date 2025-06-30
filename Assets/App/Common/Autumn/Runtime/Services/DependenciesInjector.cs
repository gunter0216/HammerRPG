using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.Autumn.Runtime.Attributes;

namespace App.Common.Autumn.Runtime.Services
{
    public class DependenciesInjector
    {
        public void InjectDependencies(
            Dictionary<RuntimeTypeHandle, object> services,
            Dictionary<RuntimeTypeHandle, List<object>> interfaces,
            Dictionary<RuntimeTypeHandle, Func<object>> transients)
        {
            foreach (var service in services)
            {
                var serviceType = Type.GetTypeFromHandle(service.Key);
                // var serviceType = service.Key;
                var fields = serviceType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var field in fields)
                {
                    var injectAttribute = field.GetCustomAttribute<InjectAttribute>();
                    if (injectAttribute == null)
                    {
                        continue;
                    }

                    var fieldType = field.FieldType;
                    object instance; 
                    if (fieldType.IsInterface)
                    {
                        if (!interfaces.TryGetValue(fieldType.TypeHandle, out var instanceList))
                        {
                            throw new ArgumentException($"Cant inject {fieldType} in {serviceType.Name}");
                        }

                        instance = instanceList.First();
                    }
                    else if (FieldIsListInterfaces(fieldType))
                    {
                        var genericType = fieldType.GenericTypeArguments[0];
                        if (!interfaces.TryGetValue(genericType.TypeHandle, out var instanceList))
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
                        if (!services.TryGetValue(fieldType.TypeHandle, out instance))
                        {
                            throw new ArgumentException($"Cant inject {fieldType} in {serviceType.Name}");
                        }
                        // throw new ArgumentException($"Cant inject {fieldType} in {serviceType.FullName}. Type can be only interface or list interfaces.");
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