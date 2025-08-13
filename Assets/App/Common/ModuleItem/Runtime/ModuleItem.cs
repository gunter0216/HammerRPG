using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.ModuleItem.Runtime.Services;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime
{
    public class ModuleItem : IModuleItem
    {
        private readonly IModulesHolder m_ModulesHolder;
        private readonly IModuleItemData m_Data;
        private readonly IModuleItemConfig m_Config;
        private readonly DataReference m_ReferenceSelf;

        public string Id => m_Config.Id;
        public DataReference ReferenceSelf => m_ReferenceSelf;
        internal IModuleItemData Data => m_Data;

        public ModuleItem(
            IModulesHolder modulesHolder,
            IModuleItemConfig moduleItemConfig, 
            IModuleItemData moduleItemData, 
            DataReference referenceSelf)
        {
            m_ModulesHolder = modulesHolder;
            m_Config = moduleItemConfig;
            m_Data = moduleItemData;
            m_ReferenceSelf = referenceSelf;
        }

        public bool Initialize()
        {
            return false;
            // m_ModuleDatas = new List<IModuleData>(m_Data.ModuleRefs.Count);
            // return m_ModulesHolder.InjectModules(m_Data.ModuleRefs, m_ModuleDatas);
        }

        public bool AddDataModule(IModuleData data)
        {
            return m_ModulesHolder.AddModule(data);
        }
        
        public bool RemoveDataModule(IModuleData data)
        {
            return m_ModulesHolder.RemoveModule(data);
        }
        
        public Optional<T> GetDataModule<T>() where T : class, IModuleData
        {
            return m_ModulesHolder.GetModule<T>();
        }
        
        public bool HasDataModule<T>() where T : class, IModuleData
        {
            return m_ModulesHolder.HasModule<T>();
        }

        public bool HasTag(long tag)
        {
            return m_Config.HasTag(tag);
        }

        public Optional<T> GetConfigModule<T>() where T : class, IModuleConfig
        {
            return m_Config.GetModule<T>();
        }

        public bool TryGetConfigModule<T>(out T config) where T : class, IModuleConfig
        {
            return m_Config.TryGetModule<T>(out config);
        }

        public bool HasConfigModule<T>() where T : class, IModuleConfig
        {
            return m_Config.HasModule<T>();
        }
    }
}