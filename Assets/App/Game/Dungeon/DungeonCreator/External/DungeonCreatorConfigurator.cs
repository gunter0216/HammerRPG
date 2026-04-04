using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Dungeon.DungeonCreator.Runtime.Chest;
using App.Game.Modules.Door.Runtime.Config;

namespace App.Game.Dungeon.DungeonCreator.External
{
    [Configurator(DIContext.CoreContext)]
    public class DungeonCreatorConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<Runtime.DungeonCreator>();

            RegisterFSM<Runtime.DungeonCreator>(FSMStage.CoreInitStage, StageOrders.DungeonCreator);
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalDungeonCreatorConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<KeyContainerData>();
        }
    }
}