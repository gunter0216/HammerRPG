using App.Common.DataContainer.Runtime;

namespace App.Common.ModuleItem.Runtime.Fabric.Interfaces
{
    public interface IModuleItemCreator
    {
        ModuleItemResult<IModuleItem> Create(string id);
        ModuleItemResult<IModuleItem> Create(DataReference dataReference);
    }
}