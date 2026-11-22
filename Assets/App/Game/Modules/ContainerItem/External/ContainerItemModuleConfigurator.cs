using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.ContainerItem.Runtime;
using App.Game.Modules.ContainerItem.Runtime.Data;

namespace App.Game.Modules.ContainerItem.External
{
    [Configurator(DIContext.CoreContext)]
    public class ContainerItemModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ContainerItemModuleSystem>();
        }
    }

    [Configurator(DIContext.GlobalContext)]
    public class GlobalContainerItemModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<ContainerItemContainerData>();
        }
    }
}
