using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Collection;
using App.Game.GameTiles.External.Config.Model;
using App.Game.ModuleItemType.Runtime.Config.Converter;

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