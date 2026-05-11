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
        [Serializable]
        public class RoomConfigVariant
        {
            [SerializeField, JsonProperty("asset")]
            // public string AssetKey;
            public GameObject AssetKey;
        
            [SerializeField, JsonProperty("asset")]
            public Vector2Int[] OutputDoors;
        }
        
        [SerializeField, JsonProperty("asset")]
        private RoomType _roomType;
        
        [SerializeField, JsonProperty("asset")]
        private Vector2Int _size;
        
        [SerializeField, JsonProperty("asset")]
        private Vector2Int _inputDoor;
        
        [SerializeField, JsonProperty("asset")]
        private RoomConfigVariant[] _variants;

        // public string AssetKey => _variants[0].AssetKey;
        public GameObject AssetKey => _variants[0].AssetKey;

        public RoomType RoomType => _roomType;

        public App.Common.Algorithms.Runtime.Vector2Int Size => new(_size.x, _size.y);

        public App.Common.Algorithms.Runtime.Vector2Int[] OutputDoors
        {
            get
            {
                var outputDoors= _variants[0].OutputDoors;
                var doors = new App.Common.Algorithms.Runtime.Vector2Int[outputDoors.Length];
                for (int i = 0; i < outputDoors.Length; ++i)
                {
                    var door = outputDoors[i];
                    doors[i] = new App.Common.Algorithms.Runtime.Vector2Int(door.x, door.y);
                }

                return doors;
            }
        }

        public App.Common.Algorithms.Runtime.Vector2Int InputDoor => new(_inputDoor.x, _inputDoor.y);
    }
}