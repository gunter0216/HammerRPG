using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime
{
    public interface IModuleItem
    {
        string Id { get; }
        
        bool AddDataModule(IModuleData data);
        bool RemoveDataModule(IModuleData data);
        Optional<T> GetDataModule<T>() where T : class, IModuleData;
        bool HasDataModule<T>() where T : class, IModuleData;
        
        bool HasTag(long tag);
        
        Optional<T> GetConfigModule<T>() where T : class, IModuleConfig;
        bool TryGetConfigModule<T>(out T config) where T : class, IModuleConfig;
        bool HasConfigModule<T>() where T : class, IModuleConfig;
    }
}