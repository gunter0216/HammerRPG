using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Fabric.Interfaces
{
    public interface ICreateModuleItemHandler
    {
        Optional<IModuleItem> Handle(IModuleItem moduleItem);
    }
}