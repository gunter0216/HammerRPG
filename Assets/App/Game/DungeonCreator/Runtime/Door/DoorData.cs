using System;
using App.Common.Algorithms.Runtime;
using App.Common.DataContainer.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using Newtonsoft.Json;

namespace App.Game.DungeonCreator.Runtime.Door
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DoorData
    {
        [JsonProperty("reference")] 
        private DataReference _reference;

        [JsonProperty("position")] 
        private Vector2Int _position;
        
        [JsonProperty("required_key")] 
        private DungeonKeyData _requiredKey;
        
        [JsonProperty("is_closed")] 
        private bool _isClosed;

        public DoorData()
        {
            
        }
        
        public DataReference Reference
        {
            get => _reference;
            set => _reference = value;
        }

        public Vector2Int Position
        {
            get => _position;
            set => _position = value;
        }

        public DungeonKeyData RequiredKey
        {
            get => _requiredKey;
            set => _requiredKey = value;
        }

        public bool IsClosed
        {
            get => _isClosed;
            set => _isClosed = value;
        }
    }
}