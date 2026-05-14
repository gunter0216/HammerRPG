using System;
using App.Common.Algorithms.Runtime;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    [Serializable]
    public class RoomConfigVariant
    {
        [JsonProperty("id")]
        private string _id;
        
        [JsonProperty("assetKey")] 
        private string _assetKey;
        
        [JsonProperty("outputDoors")] 
        private Vector2Int[] _outputDoors;
            
        public string AssetKey => _assetKey;

        public Vector2Int[] OutputDoors => _outputDoors;

        public string ID => _id;
    }
}