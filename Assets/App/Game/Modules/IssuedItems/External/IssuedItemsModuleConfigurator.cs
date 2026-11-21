using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.IssuedItems.Runtime;
using App.Game.Modules.IssuedItems.Runtime.Data;

namespace App.Game.Modules.IssuedItems.External
{
    [Configurator(DIContext.CoreContext)]
    public class IssuedItemsModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<IssuedItemsModuleSystem>();
        }
    }

    [Configurator(DIContext.GlobalContext)]
    public class GlobalIssuedItemsModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<IssuedItemsContainerData>();
        }
    }
}
