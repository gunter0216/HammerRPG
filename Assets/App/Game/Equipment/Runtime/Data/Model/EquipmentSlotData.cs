using System;
using App.Common.DataContainer.Runtime;
using Newtonsoft.Json;

namespace App.Game.Equipment.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class EquipmentSlotData
    {
        [JsonProperty("slot")]
        private string m_SlotKey;
        
        [JsonProperty("dataReference")]
        private DataReference m_DataReference;

        public DataReference DataReference
        {
            get => m_DataReference;
            set => m_DataReference = value;
        }

        public string SlotKey
        {
            get => m_SlotKey;
            set => m_SlotKey = value;
        }

        public EquipmentSlotData()
        {
        }

        public EquipmentSlotData(string slotKey, DataReference dataReference)
        {
            m_SlotKey = slotKey;
            m_DataReference = dataReference;
        }
    }
}