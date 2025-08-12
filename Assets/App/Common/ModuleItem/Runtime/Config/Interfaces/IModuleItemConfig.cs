using App.Common.Utilities.Utility.Runtime;

namespace Assets.App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IModuleItemConfig
    {
        string Id { get; }
        
        bool HasTag(long tag);
        
        Optional<T> GetModule<T>() where T : class, IModuleConfig;
        bool TryGetModule<T>(out T config) where T : class, IModuleConfig;
        bool HasModule<T>() where T : class, IModuleConfig;
    }
}