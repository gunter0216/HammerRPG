using System;
using Newtonsoft.Json;

namespace App.Game.Inventory.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryGroupData
    {
        [JsonProperty("items")] private InventoryItemData[] m_Items;
        [JsonProperty("key")] private string m_Key;

        public InventoryItemData[] Items
        {
            get => m_Items;
            set => m_Items = value;
        }

        public string Key
        {
            get => m_Key;
            set => m_Key = value;
        }
    }
}