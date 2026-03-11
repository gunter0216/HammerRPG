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
        [JsonProperty("m_UID")] 
        private readonly int m_UID;
        private Vector2Int m_Position;
        private Vector2Int m_Size;
        private readonly List<DungeonKeyData> m_ContainsDoorKeys;
        private readonly List<RoomConnection> m_Connections;
        private DungeonKeyData m_RequiredKey;
        private bool m_IsMainPath;
        private List<TileData> _tiles;

        public Vector2Int Position => m_Position;

        public Vector2Int Size => m_Size;
        public int Width => m_Size.X;
        public int Height => m_Size.Y;

        public int UID => m_UID;

        public List<TileData> Tiles
        {
            get => _tiles;
            set => _tiles = value;
        }

        public RoomData()
        {
            
        }
        
        public RoomData(DungeonGenerationRoom generationRoom)
        {
            m_UID = generationRoom.UID;
            m_Position = generationRoom.Position;
            m_Size = generationRoom.Size;
            m_ContainsDoorKeys = new List<DungeonKeyData>(generationRoom.ContainsDoorKeys);
            m_Connections = new List<RoomConnection>(generationRoom.Connections);
            m_RequiredKey = generationRoom.RequiredKey;
            m_IsMainPath = generationRoom.IsMainPath;
        }
    }
}