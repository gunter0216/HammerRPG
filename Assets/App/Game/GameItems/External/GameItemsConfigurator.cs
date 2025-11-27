using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.GameItems.External.Config;
using App.Game.Update.External;

namespace App.Game.GameItems.External
{
    [Configurator(DIContext.CoreContext)]
    public class GameItemsConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<GameItemsManager>();

            RegisterFSM<GameItemsManager>(FSMStage.CoreInitStage, StageOrders.GameItems);
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalGameItemsConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<StubCreateModuleItemHandler>();
        }
    }
}