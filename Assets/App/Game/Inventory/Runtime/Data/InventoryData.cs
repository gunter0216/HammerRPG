using System;
using App.Common.Data.Runtime;
using Newtonsoft.Json;

namespace App.Game.Inventory.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryData : IData
    {
        [JsonProperty("count_slots")] private int m_CountSlots;

        public int CountSlots
        {
            get => m_CountSlots;
            set => m_CountSlots = value;
        }
        
        public string Name()
        {
            return nameof(InventoryData);
        }
    }
}