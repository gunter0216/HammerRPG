using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.AI.External
{
    [Configurator(DIContext.CoreContext)]
    public class AIControllerConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<AIController>();

            RegisterFSM<AIController>(FSMStage.CoreInitStage, StageOrders.AIController);
            RegisterUpdate<AIController>(UpdateStage.AIController);
        }
    }
}