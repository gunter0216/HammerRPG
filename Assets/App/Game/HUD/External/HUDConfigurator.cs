using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Game.GameMenu.External
{
    [Configurator(DIContext.CoreContext)]
    public class HUDConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<HUDController>();

            RegisterFSM<HUDController>(FSMStage.CoreInitStage, StageOrders.HUD);
        }
    }
}