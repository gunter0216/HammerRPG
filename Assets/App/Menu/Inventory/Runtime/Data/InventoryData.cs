using System;
using App.Common.Data.Runtime;
using App.Common.HammerDI.Runtime.Attributes;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Menu.Inventory.Runtime.Data
{
    [Singleton]
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

        public void WhenCreateNewData()
        {
            Debug.LogError($"created first time");
        }

        public void BeforeSerialize()
        {
            
        }
    }
}