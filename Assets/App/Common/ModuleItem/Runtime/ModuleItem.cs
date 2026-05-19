using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.ModuleItem.Runtime.Services;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime
{
    public partial class ModuleItem : IModuleItem
    {
        private readonly IModulesHolder _modulesHolder;
        private readonly IModuleItemData _data;
        private readonly IModuleItemConfig _config;
        private readonly DataReference _referenceSelf;

        private List<IModule> _modules;
        
        public string Id => _config.Id;
        public DataReference ReferenceSelf => _referenceSelf;
        internal IModuleItemData Data => _data;

        public ModuleItem(
            IModulesHolder modulesHolder,
            IModuleItemConfig moduleItemConfig, 
            IModuleItemData moduleItemData, 
            DataReference referenceSelf)
        {
            _modulesHolder = modulesHolder;
            _config = moduleItemConfig;
            _data = moduleItemData;
            _referenceSelf = referenceSelf;
        }

        public bool AddDataModule(IModuleData data)
        {
            return _modulesHolder.AddModule(data);
        }
        
        public bool RemoveDataModule(IModuleData data)
        {
            return _modulesHolder.RemoveModule(data);
        }
        
        public Optional<T> GetDataModule<T>() where T : class, IModuleData
        {
            return _modulesHolder.GetModule<T>();
        }

        public bool TryGetDataModule<T>(out T data) where T : class, IModuleData
        {
            return _modulesHolder.TryGetModule<T>(out data);
        }

        public bool HasDataModule<T>() where T : class, IModuleData
        {
            return _modulesHolder.HasModule<T>();
        }

        public bool HasTag(long tag)
        {
            return _config.HasTag(tag);
        }

        public Optional<T> GetConfigModule<T>() where T : class, IModuleConfig
        {
            return _config.GetModule<T>();
        }

        public bool TryGetConfigModule<T>(out T config) where T : class, IModuleConfig
        {
            return _config.TryGetModule<T>(out config);
        }

        public bool HasConfigModule<T>() where T : class, IModuleConfig
        {
            return _config.HasModule<T>();
        }

        internal bool Destroy()
        {
            return _modulesHolder.Destroy();
        }
    }
}