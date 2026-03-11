using System;
using App.Common.Algorithms.Runtime;
using App.Common.DataContainer.Runtime;
using Newtonsoft.Json;

namespace App.Generation.DungeonCreator.Runtime.Tiles
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TileData
    {
        [JsonProperty("reference")] 
        private DataReference _reference;

        [JsonProperty("position")] 
        private Vector2Int _position;

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
    }
}