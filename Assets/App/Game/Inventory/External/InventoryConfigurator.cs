using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Runtime.Collection;
using App.Common.Data.External;
using App.Game.Inventory.External.Data;
using App.Game.Inventory.Runtime.Data;

namespace App.Game.Inventory.External
{
    [Configurator]
    public class InventoryConfigurator : IConfigurator
    {
        public void Configuration(IConfigurationCollection collection)
        {
            DataManagerProxy.RegisterDataType<InventoryData>();
        }
    }
}