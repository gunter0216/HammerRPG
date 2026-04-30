using System.Collections.Generic;
using App.Game.Inventory.Runtime.Config.Dto;

namespace App.Game.Inventory.Runtime.Config.Model
{
    public class InventoryConfig : IInventoryConfig
    {
        private readonly int m_Cols;
        private readonly int m_Rows;

        public InventoryConfig(InventoryConfigDto dto)
        {
            m_Cols = dto.Cols;
            m_Rows = dto.Rows;
        }

        public int Cols => m_Cols;
        public int Rows => m_Rows;
    }
}
