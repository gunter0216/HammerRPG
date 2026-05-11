using System;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    [Serializable]
    public class RoomConfigVariant
    {
        [SerializeField, JsonProperty("asset")]
        public GameObject _assetKey;
        
        [SerializeField, JsonProperty("asset")]
        public Vector2Int[] _outputDoors;
            
        public GameObject AssetKey => _assetKey;
            
        public App.Common.Algorithms.Runtime.Vector2Int[] OutputDoors
        {
            get
            {
                var outputDoors = _outputDoors;
                var doors = new App.Common.Algorithms.Runtime.Vector2Int[outputDoors.Length];
                for (int i = 0; i < outputDoors.Length; ++i)
                {
                    var door = outputDoors[i];
                    doors[i] = new App.Common.Algorithms.Runtime.Vector2Int(door.x, door.y);
                }

                return doors;
            }
        }
    }
}