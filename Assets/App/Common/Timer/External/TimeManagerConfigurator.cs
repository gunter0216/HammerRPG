using App.Common.FSM.External;
using App.Common.Timer.Runtime;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Common.Timer.External
{
    [Configurator(DIContext.CoreContext)]
    public class TimeManagerConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<TimeManager>();

            RegisterFSM<TimeManager>(FSMStage.CoreInitStage, StageOrders.TimeManager);

            RegisterUpdate<TimeManager>(UpdateStage.TimeManager);
        }
    }
}