using System;
using System.Collections.Generic;

namespace App.Game.Inventory.Runtime.Config
{
    public class InventoryConfig
    {
        private readonly List<InventoryGroup> m_Groups;
        private readonly int m_Cols;
        private readonly int m_SlotWidth;
        private readonly int m_SlotHeight;
        private readonly int m_Rows;

        public InventoryConfig(List<InventoryGroup> groups, int cols, int slotWidth, int slotHeight, int rows)
        {
            m_Groups = groups;
            m_Cols = cols;
            m_SlotWidth = slotWidth;
            m_SlotHeight = slotHeight;
            m_Rows = rows;
        }

        public List<InventoryGroup> Groups => m_Groups;
        public int Cols => m_Cols;
        public int SlotWidth => m_SlotWidth;
        public int SlotHeight => m_SlotHeight;
        public int Rows => m_Rows;
    }
}
