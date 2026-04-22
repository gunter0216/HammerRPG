using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.Interactions.External
{
    [Configurator(DIContext.CoreContext)]
    public class InteractionConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<InteractionController>();

            RegisterFSM<InteractionController>(FSMStage.CoreInitStage, StageOrders.Interactions);

            RegisterUpdate<InteractionController>(UpdateStage.Interactions);
        }
    }
}