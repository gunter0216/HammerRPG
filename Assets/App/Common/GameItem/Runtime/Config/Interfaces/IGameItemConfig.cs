using App.Common.Utility.Runtime;

namespace App.Common.GameItem.Runtime.Config.Interfaces
{
    public interface IGameItemConfig
    {
        string Id { get; }
        
        bool HasTag(long tag);
        
        Optional<T> GetModule<T>() where T : class, IModuleConfig;
        bool TryGetModule<T>(out T config) where T : class, IModuleConfig;
        bool HasModule<T>() where T : class, IModuleConfig;
    }
}