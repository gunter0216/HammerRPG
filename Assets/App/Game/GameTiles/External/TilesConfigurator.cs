using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Collection;
using App.Common.Data.External;
using App.Game.GameTiles.External.Config.Data;

namespace App.Game.GameTiles.External
{
    [Configurator]
    public class TilesConfigurator : IConfigurator
    {
        public void Configuration(IConfigurationCollection collection)
        {
            DataManagerProxy.RegisterDataType<PositionContainerData>();
            collection.AddSingleton(typeof(PositionContainerData));
        }
    }
}