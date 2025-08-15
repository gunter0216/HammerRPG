using System;
using App.Common.DataContainer.Runtime;
using Newtonsoft.Json;

namespace App.Game.Inventory.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryItemData
    {
        [JsonProperty("positionX")]
        private int m_PositionX;
        
        [JsonProperty("positionY")]
        private int m_PositionY;
        
        [JsonProperty("dataReference")]
        private DataReference m_DataReference;
        
        public int PositionX
        {
            get => m_PositionX;
            set => m_PositionX = value;
        }
        
        public int PositionY
        {
            get => m_PositionY;
            set => m_PositionY = value;
        }
        
        public DataReference DataReference
        {
            get => m_DataReference;
            set => m_DataReference = value;
        }
    }
}