using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime
{
    public interface IModuleItem
    {
        string Id { get; }
        DataReference ReferenceSelf { get; }
        
        bool AddModule<T>(T module) where T : class, IModule;
        bool RemoveModule(IModule module);
        Optional<T> GetModule<T>() where T : class, IModule;
        bool TryGetModule<T>(out T module) where T : class, IModule;
        bool HasModule<T>() where T : class, IModule;
        
        bool AddDataModule(IModuleData data);
        bool RemoveDataModule(IModuleData data);
        Optional<T> GetDataModule<T>() where T : class, IModuleData;
        bool TryGetDataModule<T>(out T data) where T : class, IModuleData;
        bool HasDataModule<T>() where T : class, IModuleData;
        
        bool HasTag(long tag);
        
        Optional<T> GetConfigModule<T>() where T : ModuleConfig;
        bool TryGetConfigModule<T>(out T config) where T : ModuleConfig;
        bool HasConfigModule<T>() where T : ModuleConfig;
    }
}