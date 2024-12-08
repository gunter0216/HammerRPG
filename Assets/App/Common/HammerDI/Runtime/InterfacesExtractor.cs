using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Common.HammerDI.Runtime
{
    public class InterfacesExtractor
    {
        public Dictionary<Type, List<object>> ExtractInterfaces(Dictionary<Type, object> services)
        {
            var typeToInterfacesList = new Dictionary<Type, List<object>>(services.Count);
            foreach (var service in services)
            {
                if (service.Key.IsInterface)
                {
                    AddInterface(typeToInterfacesList, service.Key, service.Value);
                    continue;
                }
                
                var interfaces = service.Key.GetInterfaces();
                for (int i = 0; i < interfaces.Length; ++i)
                {
                    var interfaceType = interfaces[i];
                    if (!interfaceType.IsInterface)
                    {
                        continue;
                    }

                    AddInterface(typeToInterfacesList, interfaceType, service.Value);
                }
            }

            return typeToInterfacesList;
        }

        private void AddInterface(Dictionary<Type,List<object>> typeToInterfacesList, Type interfaceType, object serviceValue)
        {
            if (!typeToInterfacesList.TryGetValue(interfaceType, out var objects))
            {
                objects = new List<object>();
                typeToInterfacesList.Add(interfaceType, objects);
            }
                    
            objects.Add(serviceValue);
        }
    }
}