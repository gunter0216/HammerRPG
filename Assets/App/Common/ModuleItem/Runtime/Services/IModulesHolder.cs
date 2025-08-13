using App.Common.ModuleItem.Runtime.Data;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Services
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