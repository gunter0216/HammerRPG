using System;
using System.Collections.Generic;
using App.Common.Data.Runtime;
using Newtonsoft.Json;

namespace App.Game.Inventory.Runtime.Data.Model
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryData : IData
    {
        [JsonProperty("groups")] private List<InventoryItemData> _items;

        public List<InventoryItemData> Items
        {
            get => _items;
            set => _items = value;
        }

        public string Name()
        {
            return nameof(InventoryData);
        }
    }
}