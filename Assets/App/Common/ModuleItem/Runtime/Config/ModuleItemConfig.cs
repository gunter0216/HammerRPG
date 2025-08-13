using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Config
{
    public class ModuleItemConfig : IModuleItemConfig, IComparable<ModuleItemConfig>
    {
        private readonly string m_Id;
        private readonly string m_Type;
        private readonly IReadOnlyList<IModuleConfig> m_Modules;
        private readonly long m_Tags;

        public string Id => m_Id;
        public string Type => m_Type;
        public IReadOnlyList<IModuleConfig> Modules => m_Modules;

        public ModuleItemConfig(
            string id, 
            long tags, 
            IReadOnlyList<IModuleConfig> modules,
            string type = "default")
        {
            m_Id = id;
            m_Tags = tags;
            m_Modules = modules;
            m_Type = type;
        }

        public bool HasTag(long tag)
        {
            return (m_Tags & tag) == tag;
        }

        public Optional<T> GetModule<T>() where T : class, IModuleConfig 
        {
            var moduleDto = m_Modules.FirstOrDefault(x => x is T);
            if (moduleDto == default)
            {
                return Optional<T>.Fail();
            }
            
            return Optional<T>.Success(moduleDto as T);
        }

        public bool TryGetModule<T>(out T config) where T : class, IModuleConfig 
        {
            var module = m_Modules.FirstOrDefault(x => x is T);
            if (module == default)
            {
                config = null;
                return false;
            }

            config = (T)module;
            return true;
        }

        public bool HasModule<T>() where T : class, IModuleConfig 
        {
            return m_Modules.Any(x => x is T);
        }
        
        public int CompareTo(ModuleItemConfig other)
        {
            return String.Compare(m_Id, other.m_Id, StringComparison.Ordinal);
        }
    }
}
