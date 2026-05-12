using System;
using App.Generation.DungeonGenerator.External.Dto.Generation;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    [Serializable]
    public class TileBasedGenerationConfig : IGenerationConfig
    {
        [SerializeField, JsonProperty("rooms")] 
        private RoomConfigAsset[] _rooms;
        
        [SerializeField, JsonProperty("maxDepth")] 
        private int _maxDepth = 2;
        
        [SerializeField, JsonProperty("maxOutputs")] 
        private int _maxOutputs = 2;
        
        [SerializeField, JsonProperty("maxIterations")] 
        private int _maxIterations = 300;

        public RoomConfigAsset[] Rooms => _rooms;

        public int MaxDepth => _maxDepth;

        public int MaxOutputs => _maxOutputs;
        public int MaxIterations => _maxIterations;
    }
}