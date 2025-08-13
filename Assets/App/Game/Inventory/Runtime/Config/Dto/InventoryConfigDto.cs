using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Game.Inventory.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryConfigDto
    {
        [JsonProperty("groups")]
        private List<InventoryGroupDto> m_Groups;
        [JsonProperty("cols")]
        private int m_Cols;
        [JsonProperty("slotWidth")]
        private int m_SlotWidth;
        [JsonProperty("slotHeight")]
        private int m_SlotHeight;
        [JsonProperty("rows")]
        private int m_Rows;

        public List<InventoryGroupDto> Groups => m_Groups;
        public int Cols => m_Cols;
        public int SlotWidth => m_SlotWidth;
        public int SlotHeight => m_SlotHeight;
        public int Rows => m_Rows;
    }
}
