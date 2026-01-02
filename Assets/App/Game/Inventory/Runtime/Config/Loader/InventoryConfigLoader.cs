using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime.Config.Dto;

namespace App.Game.Inventory.Runtime.Config.Loader
{
    public class InventoryConfigLoader
    {
        private const string m_LocalConfigKey = "InventoryConfig";
        private readonly IConfigLoader m_ConfigLoader;

        public InventoryConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<InventoryConfigDto> Load()
        {
            return m_ConfigLoader.LoadConfig<InventoryConfigDto>(m_LocalConfigKey);
        }
    }
}
