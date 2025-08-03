using System;
using App.Common.Data.Runtime;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Game.Inventory.External.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryData : IData
    {
        [SerializeField, JsonProperty("count_slots")] private int m_CountSlots;

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