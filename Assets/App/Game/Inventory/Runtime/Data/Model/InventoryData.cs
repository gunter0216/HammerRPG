using System;
using System.Collections.Generic;
using App.Common.Data.Runtime;
using Newtonsoft.Json;

namespace App.Game.Inventory.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryData : IData
    {
        [JsonProperty("items")] private List<InventoryItemData> m_Items;

        public List<InventoryItemData> Items
        {
            get => m_Items;
            set => m_Items = value;
        }

        public string Name()
        {
            return nameof(InventoryData);
        }
    }
}