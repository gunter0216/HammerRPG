using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.ContainerModule.Runtime;
using App.Game.Modules.ContainerModule.Runtime.Config;
using App.Game.Modules.ContainerModule.Runtime.Data;

namespace App.Game.Modules.ContainerModule.External
{
    [Configurator(DIContext.CoreContext)]
    public class ContainerModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ContainerModuleSystem>();
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalContainerModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ContainerModuleDtoToConfigConverter>();
            
            RegisterData<ContainerContainerData>();
        }
    }
}