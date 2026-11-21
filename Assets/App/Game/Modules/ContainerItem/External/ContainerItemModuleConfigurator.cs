using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.ContainerItem.Runtime;

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
}
