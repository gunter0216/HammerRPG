using App.Common.ModuleItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime
{
    public interface IGameItemsManager
    {
        Optional<IModuleItem> Create(string id);
        Optional<IModuleItem> Create(IModuleItemData data);
        bool Destroy(IModuleItemData data);
    }
}