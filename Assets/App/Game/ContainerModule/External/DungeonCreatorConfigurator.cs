using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.ContainerModule.Runtime;
using App.Generation.DungeonCreator.Runtime.Chest;

namespace App.Game.GameManagers.External
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