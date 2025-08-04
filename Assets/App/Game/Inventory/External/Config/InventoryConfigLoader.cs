using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;
using App.Game.Inventory.External.Dto;

namespace App.Game.Inventory.External.Config
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
