using System;
using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Tiles;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using Newtonsoft.Json;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Rooms
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class RoomData
    {
        [JsonProperty("UID")] 
        private readonly int _UID;
        
        [JsonProperty("position")]
        private readonly Vector2Int _position;
        
        [JsonProperty("rotation")]
        private readonly float _rotation;
        
        [JsonProperty("configID")]
        private readonly string _configID;
        
        [JsonProperty("variantID")]
        private readonly string _variantID;
        
        [JsonProperty("tiles")]
        private readonly List<TileData> _tiles;
        
        public Vector2Int Position => _position;

        public int UID => _UID;
        public List<TileData> Tiles => _tiles;
        public float Rotation => _rotation;

        public string ConfigID => _configID;

        public string VariantID => _variantID;

        public RoomData()
        {
            
        }
        
        public RoomData(DungeonGenerationRoom generationRoom)
        {
            _UID = generationRoom.UID;
            _position = generationRoom.Position;
            _rotation = generationRoom.RotateEuler;
            _configID = generationRoom.GenerationConfig.ID;
            _variantID = generationRoom.ConfigVariant.ID;
            _tiles = new List<TileData>();
        }
    }
}