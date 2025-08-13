using App.Common.DataContainer.Runtime;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.ModuleItem.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;
using Assets.App.Common.ModuleItem.Runtime.Data;

namespace Assets.App.Game.GameItems.Runtime
{
    public class GameModuleItem : IGameModuleItem
    {
        private readonly IModuleItem m_ModuleItem;

        public string Id => m_ModuleItem.Id;
        public DataReference ReferenceSelf => m_ModuleItem.ReferenceSelf;

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