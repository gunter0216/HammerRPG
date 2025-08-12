using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;
using Assets.App.Common.ModuleItem.Runtime.Data;

namespace Assets.App.Common.ModuleItem.Runtime
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