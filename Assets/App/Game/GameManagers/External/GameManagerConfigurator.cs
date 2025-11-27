using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.GameManagers.External
{
    [Configurator(DIContext.CoreContext)]
    public class GameManagerConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<GameManager>();

            RegisterFSM<GameManager>(FSMStage.CoreInitStage, StageOrders.GameManager);
        }
    }
}