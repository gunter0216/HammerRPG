using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Collection;
using App.Game.GameItems.Runtime.Config.DtoConverter;
using App.Game.GameTiles.External.Config.Model;

namespace App.Game.GameItems.External
{
    [Configurator]
    public class GameItemsConfigurator : IConfigurator
    {
        public void Configuration(IConfigurationCollection collection)
        {
            collection.AddSingleton(typeof(GameItemTypeModuleDtoToConfigConverter));
        }
    }
}