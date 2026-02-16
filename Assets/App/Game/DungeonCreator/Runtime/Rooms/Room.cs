using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Game.DungeonCreator.Runtime.Tiles;
using App.Game.GameManagers.External.Room;

namespace App.Game.DungeonCreator.Runtime.Rooms
{
    public class Room
    {
        private readonly RoomData m_Data;
        private List<Tile> m_Tiles;

        public Room(RoomData data)
        {
            m_Data = data;
        }

        public RoomData Data => m_Data;

        public List<Tile> Tiles
        {
            get => m_Tiles;
            set => m_Tiles = value;
        }
        
        public int Width => m_Data.Width;
        public int Height => m_Data.Height;
        
        public Vector2 GetCenter()
        {
            return new Vector2(m_Data.Position.X + m_Data.Width * 0.5f, m_Data.Position.Y + m_Data.Height * 0.5f);
        }
    }
}