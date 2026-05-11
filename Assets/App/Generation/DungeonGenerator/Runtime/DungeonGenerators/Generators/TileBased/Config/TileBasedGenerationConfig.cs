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

        public RoomConfigAsset[] Rooms => _rooms;
        public int MaxDepth => 1;
    }
}