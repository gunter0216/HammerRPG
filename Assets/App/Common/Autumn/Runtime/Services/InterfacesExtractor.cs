using System;
using System.Collections.Generic;

namespace App.Common.Autumn.Runtime.Services
{
    public class InterfacesExtractor
    {
        public Dictionary<RuntimeTypeHandle, List<object>> CreateInterfaceToInstances(IReadOnlyList<object> services)
        {
            var interfaceToInstances = new Dictionary<RuntimeTypeHandle, List<object>>(services.Count);
            foreach (var service in services)
            {
                var type = service.GetType();
                var interfaces = type.GetInterfaces();
                foreach (var interfaceType in interfaces)
                {
                    if (!interfaceType.IsInterface)
                    {
                        continue;
                    }

                    AddInterface(interfaceToInstances, interfaceType.TypeHandle, service);
                }
            }

            return interfaceToInstances;
        }

        
        // public IDictionary<RuntimeTypeHandle, IReadOnlyList<object>> ExtractInterfaces(
        //     IDictionary<RuntimeTypeHandle, object> services)
        // {
        //     var typeToInterfacesList = new Dictionary<Type, List<object>>(services.Count);
        //     foreach (var service in services)
        //     {
        //         if (service.Key.IsInterface)
        //         {
        //             AddInterface(typeToInterfacesList, service.Key, service.Value);
        //             continue;
        //         }
        //         
        //         var interfaces = service.Key.GetInterfaces();
        //         for (int i = 0; i < interfaces.Length; ++i)
        //         {
        //             var interfaceType = interfaces[i];
        //             if (!interfaceType.IsInterface)
        //             {
        //                 continue;
        //             }
        //
        //             AddInterface(typeToInterfacesList, interfaceType, service.Value);
        //         }
        //     }
        //
        //     return typeToInterfacesList;
        // }

        private void AddInterface(
            Dictionary<RuntimeTypeHandle, List<object>> interfaceToInstances, 
            RuntimeTypeHandle interfaceType, 
            object service)
        {
            if (!interfaceToInstances.TryGetValue(interfaceType, out var services))
            {
                services = new List<object>();
                interfaceToInstances.Add(interfaceType, services);
            }
                    
            services.Add(service);
        }
    }
}