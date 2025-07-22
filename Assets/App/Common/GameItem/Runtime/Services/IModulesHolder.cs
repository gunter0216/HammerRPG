using App.Common.GameItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.Runtime.Services
{
    public interface IModulesHolder
    {
        bool AddModule(IModuleData data);
        bool RemoveModule(IModuleData data);
        Optional<T> GetModule<T>() where T : IModuleData;
        bool TryGetModule<T>(out T data) where T : IModuleData;
        bool HasModule<T>() where T : IModuleData;
    }
}