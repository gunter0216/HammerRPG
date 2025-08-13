using App.Common.DataContainer.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Fabric.Interfaces
{
    public interface IModuleItemCreator
    {
        Optional<IModuleItem> Create(string id);
        Optional<IModuleItem> Create(DataReference dataReference);
    }
}