using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Fabric.Interfaces
{
    public interface ICreateModuleItemHandler : ISortIndex
    {
        Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem);
    }
}