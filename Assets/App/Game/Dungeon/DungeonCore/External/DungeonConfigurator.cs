using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Game.Dungeon.DungeonCore.External
{
    [Configurator(DIContext.CoreContext)]
    public class DungeonConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<DungeonController>();

            RegisterFSM<DungeonController>(FSMStage.CoreInitStage, StageOrders.DungeonController);
        }
    }
}