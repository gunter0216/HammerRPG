using System;
using App.Generation.DungeonGenerator.External.Dto.Generation;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    [Serializable]
    public class TileBasedGenerationConfig : IGenerationConfig
    {
        [SerializeField] 
        private RoomConfigAsset[] _rooms;
        
        [SerializeField] 
        private int _maxDepth = 2;
        
        [SerializeField] 
        private int _maxOutputs = 2;

        public RoomConfigAsset[] Rooms => _rooms;

        public int MaxDepth => _maxDepth;

        public int MaxOutputs => _maxOutputs;
    }
}