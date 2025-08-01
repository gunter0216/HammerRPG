using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Fabric.Interfaces
{
    public interface ICreateModuleItemHandler
    {
        Optional<IModuleItem> Handle(IModuleItem moduleItem);
    }
}