using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.GameMenu.External
{
    [Configurator(DIContext.CoreContext)]
    public class GameMenuConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<GameMenuController>();

            RegisterFSM<GameMenuController>(FSMStage.CoreInitStage, StageOrders.GameMenu);
            
            RegisterUpdate<GameMenuController>(UpdateStage.GameMenu);
        }
    }
}