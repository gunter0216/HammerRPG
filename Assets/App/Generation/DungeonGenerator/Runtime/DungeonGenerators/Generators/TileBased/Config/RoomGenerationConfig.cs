using System;
using App.Common.Algorithms.Runtime;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    [Serializable]
    public class RoomGenerationConfig
    {
        [JsonProperty("id")]
        private string _id;
        
        [JsonProperty("type")]
        private string _roomType;
        
        [JsonProperty("size")]
        private Vector2Int _size;
        
        [JsonProperty("inputDoor")]
        private Vector2Int _inputDoor;
        
        [JsonProperty("variants")]
        private RoomConfigVariant[] _variants;

        public RoomType RoomType
        {
            get
            {
                Enum.TryParse<RoomType>(_roomType, out var type);
                return type;
            }
        }

        public Vector2Int Size => new(_size.X, _size.Y);

        public Vector2Int InputDoor => new(_inputDoor.X, _inputDoor.Y);

        public RoomConfigVariant[] Variants => _variants;

        public string ID => _id;
    }
}