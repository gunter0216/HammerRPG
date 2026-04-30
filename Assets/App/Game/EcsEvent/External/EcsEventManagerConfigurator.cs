using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.EcsEvent.External
{
    [Configurator(DIContext.CoreContext)]
    public class EcsEventManagerConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<EcsEventManager>();

            RegisterFSM<EcsEventManager>(FSMStage.CoreInitStage, StageOrders.EcsEventManager);

            RegisterUpdate<EcsEventManager>(UpdateStage.EcsEventManager);
        }
    }
}