using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Modules.TilePosition.Runtime;

namespace App.Game.Modules.TilePosition.External
{
    [Configurator(DIContext.GlobalContext)]
    public class TilePositionModuleConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<TilePositionContainerData>();
        }
    }
}