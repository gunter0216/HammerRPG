using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Inventory.External;
using App.Game.Update.External;

namespace App.Game.StatusBar.External
{
    [Configurator(DIContext.CoreContext)]
    public class StatusBarConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<StatusBarController>();

            RegisterFSM<StatusBarController>(FSMStage.CoreInitStage, StageOrders.StatusBar);
            RegisterUpdate<StatusBarController>(UpdateStage.StatusBar);
        }
    }
}