using System;
using App.Common.Data.Runtime;
using App.Common.Data.Runtime.Attributes;
using App.Game.Inventory.Runtime.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Game.Inventory.External.Data
{
    [Data]
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryData : IData, IInventoryData
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

        public void WhenCreateNewData()
        {
            Debug.LogError($"created first time");
        }

        public void BeforeSerialize()
        {
            
        }
    }
}