using System;
using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Generation.DungeonCreator.Runtime.Tiles;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.Matrix;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using Newtonsoft.Json;

namespace App.Game.GameManagers.External.Room
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class RoomData
    {
        [JsonProperty("UID")] 
        private readonly int _UID;
        
        [JsonProperty("position")]
        private readonly Vector2Int _position;
        
        [JsonProperty("size")]
        private readonly Vector2Int _size;
        
        [JsonProperty("tiles")]
        private readonly List<TileData> _tiles;
        
        [JsonProperty("floors")]
        private readonly List<RectInt> _floors;

        public Vector2Int Position => _position;

        public Vector2Int Size => _size;
        public int Width => _size.X;
        public int Height => _size.Y;
        public int UID => _UID;
        public List<TileData> Tiles => _tiles;
        public List<RectInt> Floors => _floors;

        public RoomData()
        {
            
        }
        
        public RoomData(DungeonGenerationRoom generationRoom)
        {
            _UID = generationRoom.UID;
            _position = generationRoom.Position;
            _size = generationRoom.Size;
            _tiles = new List<TileData>();
            _floors = new List<RectInt>();
        }
    }
}