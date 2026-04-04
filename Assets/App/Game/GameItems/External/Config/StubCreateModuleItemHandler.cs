using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Fabric.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.GameItems.External.Config
{
    public class StubCreateModuleItemHandler : ICreateModuleItemHandler
    {
        public Optional<IModuleItem> OnItemCreated(IModuleItem moduleItem)
        {
            return Optional<IModuleItem>.Success(moduleItem);
        }
    }
}