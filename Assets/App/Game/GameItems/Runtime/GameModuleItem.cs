using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Game.GameItems.Runtime
{
    public class GameModuleItem : IGameModuleItem
    {
        private readonly IModuleItem m_ModuleItem;

        public string Id => m_ModuleItem.Id;

        public GameModuleItem(IModuleItem moduleItem)
        {
            m_ModuleItem = moduleItem;
        }

        public bool AddDataModule(IModuleData data)
        {
            return m_ModuleItem.AddDataModule(data);
        }

        public bool RemoveDataModule(IModuleData data)
        {
            return m_ModuleItem.RemoveDataModule(data);
        }

        public Optional<T> GetDataModule<T>() where T : class, IModuleData
        {
            return m_ModuleItem.GetDataModule<T>();
        }

        public bool HasDataModule<T>() where T : class, IModuleData
        {
            return m_ModuleItem.HasDataModule<T>();
        }

        public bool HasTag(long tag)
        {
            return m_ModuleItem.HasTag(tag);
        }

        public Optional<T> GetConfigModule<T>() where T : class, IModuleConfig
        {
            return m_ModuleItem.GetConfigModule<T>();
        }

        public bool TryGetConfigModule<T>(out T config) where T : class, IModuleConfig
        {
            return m_ModuleItem.TryGetConfigModule(out config);
        }

        public bool HasConfigModule<T>() where T : class, IModuleConfig
        {
            return m_ModuleItem.HasConfigModule<T>();
        }
    }
}