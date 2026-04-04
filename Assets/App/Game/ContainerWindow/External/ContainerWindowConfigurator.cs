using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Game.ContainerWindow.External
{
    [Configurator(DIContext.CoreContext)]
    public class ContainerWindowConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ContainerWindowController>();
            BindSingle<ContainerToInventoryController>();

            RegisterFSM<ContainerWindowController>(FSMStage.CoreInitStage, StageOrders.Container);
            RegisterFSM<ContainerToInventoryController>(FSMStage.CoreInitStage, StageOrders.Container);
        }
    }
}