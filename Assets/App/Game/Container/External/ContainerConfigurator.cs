using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Container.Runtime;
using App.Game.Container.Runtime.Data.Model;

namespace App.Game.DragItem.External
{
    [Configurator(DIContext.CoreContext)]
    public class ContainerConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<ContainerController>();

            RegisterFSM<ContainerController>(FSMStage.CoreInitStage, StageOrders.Container);
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalContainerConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<ContainersData>();
        }
    }
}