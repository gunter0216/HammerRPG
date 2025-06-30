using System;
using System.Collections.Generic;
using System.Reflection;

namespace App.Common.AssemblyManager.Runtime
{
    public class AssemblyProviderBuilder : IAssemblyProviderBuilder
    {
        private readonly Dictionary<Type, List<AttributeNode>> m_AttributeToTypes;
        private readonly List<Type> m_Attributes;
        private bool m_Inherit = false;

        public AssemblyProviderBuilder()
        {
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
            var assembly = Assembly.GetCallingAssembly();
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
            
            return new AssemblyProvider(m_AttributeToTypes);
        }

        private bool HasAttribute(Type type, Type attribute)
        {
            return type.IsDefined(attribute, m_Inherit);
        }
    }
}