using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.GameTiles.External.Config.Data;
using App.Game.GameTiles.External.Config.Model;

namespace App.Game.GameTiles.External
{
    [Configurator(DIContext.CoreContext)]
    public class TilesConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<TilesController>();

            RegisterFSM<TilesController>(FSMStage.CoreInitStage, StageOrders.Tiles);
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalTilesConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<SpriteModuleDtoToConfigConverter>();
            
            RegisterData<PositionContainerData>();
        }
    }
}