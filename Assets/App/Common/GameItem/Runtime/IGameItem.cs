using App.Common.GameItem.Runtime.Config.Interfaces;
using App.Common.GameItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.Runtime
{
    public interface IGameItem
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