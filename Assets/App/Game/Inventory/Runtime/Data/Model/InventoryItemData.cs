using System;
using App.Common.DataContainer.Runtime;
using Newtonsoft.Json;

namespace App.Game.Inventory.Runtime.Data.Model
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryItemData
    {
        [JsonProperty("index")]
        private int m_Index;
        
        [JsonProperty("dataReference")]
        private DataReference m_DataReference;

        public DataReference DataReference
        {
            get => m_DataReference;
            set => m_DataReference = value;
        }

        public int Index
        {
            get => m_Index;
            set => m_Index = value;
        }

        public InventoryItemData()
        {
        }

        public InventoryItemData(int index, DataReference dataReference)
        {
            m_Index = index;
            m_DataReference = dataReference;
        }
    }
}