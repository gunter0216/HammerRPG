using App.Common.GameItem.Runtime.Config.Interfaces;
using App.Common.GameItem.Runtime.Data;
using App.Common.GameItem.Runtime.Services;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.Runtime
{
    public class GameItem : IGameItem
    {
        private readonly IModulesHolder m_ModulesHolder;
        private readonly IGameItemData m_Data;
        private readonly IGameItemConfig m_Config;
        
        public string Id => m_Config.Id;

        public GameItem(
            IModulesHolder modulesHolder,
            IGameItemConfig gameItemConfig, 
            IGameItemData gameItemData)
        {
            m_ModulesHolder = modulesHolder;
            m_Config = gameItemConfig;
            m_Data = gameItemData;
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