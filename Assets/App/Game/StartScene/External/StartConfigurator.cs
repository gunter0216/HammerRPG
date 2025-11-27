using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.StartScene.External
{
    [Configurator(DIContext.StartContext)]
    public class StartConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<StartSceneManager>();

            RegisterFSM<StartSceneManager>(FSMStage.StartInitStage, StageOrders.StartSceneManager);
        }
    }
}