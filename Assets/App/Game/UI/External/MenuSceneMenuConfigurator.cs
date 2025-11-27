using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;
using App.Menu.UI.External.Data;

namespace App.Menu.UI.External
{
    [Configurator(DIContext.MenuContext)]
    public class MenuSceneMenuConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<MenuSceneMenuController>();

            RegisterFSM<MenuSceneMenuController>(FSMStage.MenuInitStage, StageOrders.MenuSceneMenu);
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalMenuSceneMenuConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<GameRecordsData>();
        }
    }
}