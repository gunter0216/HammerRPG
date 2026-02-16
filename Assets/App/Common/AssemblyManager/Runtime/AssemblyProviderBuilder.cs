using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Common.Logger.Runtime;

namespace App.Common.AssemblyManager.Runtime
{
    public class AssemblyProviderBuilder : IAssemblyProviderBuilder
    {
        private readonly IReadOnlyList<string> _assemblyNames;
        private readonly Dictionary<Type, List<AttributeNode>> m_AttributeToTypes;
        private readonly List<Type> m_Attributes;
        private bool m_Inherit = false;

        public AssemblyProviderBuilder(IReadOnlyList<string> assemblyNames)
        {
            _assemblyNames = assemblyNames;
            m_AttributeToTypes = new Dictionary<Type, List<AttributeNode>>();
            m_Attributes = new List<Type>();
        }

        public IAssemblyProviderBuilder AddAttribute<T>() where T : Attribute
        {
            var type = typeof(T);
            m_Attributes.Add(type);
            m_AttributeToTypes.Add(type, new List<AttributeNode>());
            return this;
        }
        
        public IAssemblyProvider Build()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var targetAssemblies = loadedAssemblies
                .Where(a => _assemblyNames.Contains(a.GetName().Name))
                .ToArray();
            
            foreach (var assembly in targetAssemblies)
            {
                var allTypes = assembly.GetTypes();
                for (int i = 0; i < allTypes.Length; ++i)
                {
                    var type = allTypes[i];
                    for (int j = 0; j < m_Attributes.Count; ++j)
                    {
                        var attributeType = m_Attributes[j];
                        if (HasAttribute(type, attributeType))
                        {
                            var attribute = type.GetCustomAttribute(attributeType, m_Inherit);
                            m_AttributeToTypes[attributeType].Add(new AttributeNode(type, attribute));
                        }
                    }
                }
            }

            return new AssemblyProvider(m_AttributeToTypes);
        }

        private bool HasAttribute(Type type, Type attribute)
        {
            return type.IsDefined(attribute, m_Inherit);
        }
    }
}