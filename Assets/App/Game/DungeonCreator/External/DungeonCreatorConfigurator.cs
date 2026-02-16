using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;

namespace App.Game.GameManagers.External
{
    [Configurator(DIContext.CoreContext)]
    public class DungeonCreatorConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<Generation.DungeonCreator.Runtime.DungeonCreator>();

            RegisterFSM<Generation.DungeonCreator.Runtime.DungeonCreator>(FSMStage.CoreInitStage, StageOrders.DungeonCreator);
        }
    }
}