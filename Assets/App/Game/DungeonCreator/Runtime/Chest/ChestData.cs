using System;
using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.DataContainer.Runtime;
using Newtonsoft.Json;

namespace App.Generation.DungeonCreator.Runtime.Chest
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ChestData
    {
        [JsonProperty("reference")] 
        private DataReference _reference;
        
        [JsonProperty("position")] 
        private Vector2Int _position;
        
        [JsonProperty("items")] 
        private List<DataReference> _items;
        
        [JsonProperty("state")] 
        private int _state;

        public ChestData()
        {
            
        }
        
        public Vector2Int Position
        {
            get => _position;
            set => _position = value;
        }

        public List<DataReference> Items
        {
            get => _items;
            set => _items = value;
        }

        public DataReference Reference
        {
            get => _reference;
            set => _reference = value;
        }

        public int State
        {
            get => _state;
            set => _state = value;
        }
    }
}