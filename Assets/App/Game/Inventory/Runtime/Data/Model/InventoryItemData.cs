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
        private readonly int _index;
        
        [JsonProperty("dataReference")]
        private DataReference _dataReference;

        public DataReference DataReference
        {
            get => _dataReference;
            set => _dataReference = value;
        }

        public int Index => _index;

        public InventoryItemData()
        {
        }
        
        public InventoryItemData(int index)
        {
            _index = index;
        }

        public InventoryItemData(int index, DataReference dataReference)
        {
            _index = index;
            _dataReference = dataReference;
        }
    }
}