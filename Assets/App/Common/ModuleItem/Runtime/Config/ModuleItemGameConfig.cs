using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Configs.External;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Common.ModuleItem.Runtime.Config
{
    public class ModuleItemGameConfig : GameConfig, IModuleItemConfig, IComparable<ModuleItemGameConfig>
    {
        // Unity не сериализует readonly-поля, поэтому для m_Id/m_Type/m_Tags/m_Modules
        // readonly пришлось убрать — иначе SerializedObject их просто не видит.
        [SerializeField]
        private string m_Id;

        [SerializeField]
        private string m_Type;

        // IModuleConfig — интерфейс, поэтому для сериализации нужен конкретный
        // List<>, помеченный [SerializeReference] (полиморфная сериализация).
        [SerializeReference]
        private List<ModuleConfig> m_Modules;

        [SerializeField]
        private long m_Tags;

        public string Id => m_Id;
        public string Type => m_Type;
        public IReadOnlyList<IModuleConfig> Modules => m_Modules;

        public ModuleItemGameConfig(
            string id, 
            long tags, 
            IReadOnlyList<IModuleConfig> modules,
            string type = "default")
        {
            m_Id = id;
            m_Tags = tags;
            // m_Modules = modules != null ? new List<ModuleConfig>(modules) : new List<ModuleConfig>();
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

            // config = (T)module;
            config = null;
            return true;
        }

        public bool HasModule<T>() where T : class, IModuleConfig 
        {
            return m_Modules.Any(x => x is T);
        }

        public void AddModule(ModuleConfig module)
        {
            Debug.LogError($"AddModule {module}");
            if (module == null) return;
            m_Modules.Add(module);
        }

        public void RemoveModuleAt(int index)
        {
            if (index < 0 || index >= m_Modules.Count) return;
            m_Modules.RemoveAt(index);
        }

        
        public int CompareTo(ModuleItemGameConfig other)
        {
            return String.Compare(m_Id, other.m_Id, StringComparison.Ordinal);
        }
    }
}