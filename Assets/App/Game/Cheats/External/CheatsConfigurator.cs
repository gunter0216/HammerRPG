using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.Cheats.External
{
    [Configurator(DIContext.CoreContext)]
    public class CheatsConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<CheatsController>();
            BindSingle<OpenCheatsSystem>();

            RegisterFSM<CheatsController>(FSMStage.CoreInitStage, StageOrders.Cheats);
        }
    }
}