using System;
using System.Collections.Generic;
using App.Game.Inventory.External.Dto;

namespace App.Game.Inventory.Runtime.Config
{
    public class InventoryConfig : IInventoryConfig
    {
        private readonly List<IInventoryGroupConfig> m_Groups;
        private readonly int m_Cols;
        private readonly int m_SlotWidth;
        private readonly int m_SlotHeight;
        private readonly int m_Rows;

        public InventoryConfig(InventoryConfigDto dto)
        {
            m_Cols = dto.Cols;
            m_SlotWidth = dto.SlotWidth;
            m_SlotHeight = dto.SlotHeight;
            m_Rows = dto.Rows;
            
            m_Groups = new List<IInventoryGroupConfig>();
            foreach (var groupDto in dto.Groups)
            {
                var group = new InventoryGroupConfig(
                    groupDto.Id, 
                    groupDto.Icon,
                    groupDto.GameType);
                m_Groups.Add(group);
            }
        }

        public IReadOnlyList<IInventoryGroupConfig> Groups => m_Groups;
        public int Cols => m_Cols;
        public int SlotWidth => m_SlotWidth;
        public int SlotHeight => m_SlotHeight;
        public int Rows => m_Rows;
    }
}
