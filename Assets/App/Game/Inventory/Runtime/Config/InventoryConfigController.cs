using System.Collections.Generic;
using App.Common.Configs.Runtime;
using App.Common.Logger.Runtime;
using App.Game.Inventory.External.Config;

namespace App.Game.Inventory.Runtime.Config
{
    public class InventoryConfigController : IInventoryConfigController
    {
        private readonly IConfigLoader m_ConfigLoader;
        
        private InventoryConfig m_Config;

        public InventoryConfigController(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }
        
        public bool Initialize()
        {
            var configLoader = new InventoryConfigLoader(m_ConfigLoader);
            var dto = configLoader.Load();
            if (!dto.HasValue)
            {
                HLogger.LogError("InventoryConfig is null");
                return false;
            }
            
            var converter = new InventoryDtoToConfigConverter();
            var config = converter.Convert(dto.Value);
            if (!config.HasValue)
            {
                HLogger.LogError("InventoryConfig conversion failed");
                return false;
            }

            m_Config = config.Value;

            return true;
        }

        public IReadOnlyList<IInventoryGroupConfig> GetGroups()
        {
            return m_Config.Groups;
        }

        public int GetCols()
        {
            return m_Config.Cols;
        }

        public int GetSlotWidth()
        {
            return m_Config.SlotWidth;
        }

        public int GetSlotHeight()
        {
            return m_Config.SlotHeight;
        }

        public int GetRows()
        {
            return m_Config.Rows;
        }
    }
}