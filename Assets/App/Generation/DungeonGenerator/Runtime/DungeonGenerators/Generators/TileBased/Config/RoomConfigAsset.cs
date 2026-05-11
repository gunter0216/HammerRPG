using System;
using App.Generation.DungeonGenerator.External.Dto.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    [Serializable]
    [CreateAssetMenu(menuName = "Configs/Room", fileName = "RoomConfig", order = 1)]
    public class RoomConfigAsset : ScriptableObject
    {
        [SerializeField, JsonProperty("asset")]
        private RoomType _roomType;
        
        [SerializeField, JsonProperty("asset")]
        private Vector2Int _size;
        
        [SerializeField, JsonProperty("asset")]
        private Vector2Int _inputDoor;
        
        [SerializeField, JsonProperty("asset")]
        private RoomConfigVariant[] _variants;

        public RoomType RoomType => _roomType;

        public App.Common.Algorithms.Runtime.Vector2Int Size => new(_size.x, _size.y);

        public App.Common.Algorithms.Runtime.Vector2Int InputDoor => new(_inputDoor.x, _inputDoor.y);

        public RoomConfigVariant[] Variants => _variants;
    }
}