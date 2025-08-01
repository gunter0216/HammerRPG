using App.Common.ModuleItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Fabric.Interfaces
{
    public interface IModuleItemCreator
    {
        Optional<IModuleItem> Create(string id);
        Optional<IModuleItem> Create(IModuleItemData data);
    }
}