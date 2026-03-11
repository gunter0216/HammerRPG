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
        private DataReference m_Reference;

        [JsonProperty("position")] 
        private Vector2Int _position;

        public DataReference Reference
        {
            get => m_Reference;
            set => m_Reference = value;
        }

        public Vector2Int Position
        {
            get => _position;
            set => _position = value;
        }
    }
}