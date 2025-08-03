using System.Collections.Generic;

namespace App.Game.Inventory.Runtime.Config
{
    public class InventoryConfigController : IInventoryConfigController
    {
        private readonly IInventoryConfig m_Config;

        public InventoryConfigController(IInventoryConfig config)
        {
            m_Config = config;
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