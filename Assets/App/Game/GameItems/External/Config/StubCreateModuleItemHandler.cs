using App.Common.Autumn.Runtime.Attributes;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.ModuleItem.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Fabric.Interfaces;

namespace App.Game.GameItems.External.Config
{
    [Singleton]
    public class StubCreateModuleItemHandler : ICreateModuleItemHandler
    {
        public Optional<IModuleItem> Handle(IModuleItem moduleItem)
        {
            return Optional<IModuleItem>.Success(moduleItem);
        }
    }
}