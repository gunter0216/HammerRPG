using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime.Config.Dto;

namespace App.Game.Inventory.Runtime.Config.Loader
{
    public class InventoryConfigLoader
    {
        private const string LocalConfigKey = "InventoryConfig";
        private readonly IConfigLoader _configLoader;

        public InventoryConfigLoader(IConfigLoader configLoader)
        {
            _configLoader = configLoader;
        }

        public Optional<InventoryGameConfig> Load()
        {
            return _configLoader.LoadGameConfig<InventoryGameConfig>(LocalConfigKey);
        }
    }
}
