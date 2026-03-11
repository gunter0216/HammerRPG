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
        public Vector2Int Position => m_Data.Position;
        public Vector2Int Size => m_Data.Size;
        
        public Vector2Int LocalToWorld(int x, int y) 
        {
            return new Vector2Int(Position.X + x, Position.Y + Height - 1 - y);
        }
        
        public Vector2Int LocalToWorld(Vector2Int localPosition)
        {
            return LocalToWorld(localPosition.X, localPosition.Y);
        }
        
        public Vector2Int WorldToLocal(int x, int y) 
        {
            return new Vector2Int(x - Position.X, Position.Y - y + Height - 1);
        }

        public Vector2Int WorldToLocal(Vector2Int worldPosition)
        {
            return WorldToLocal(worldPosition.X, worldPosition.Y);
        }
        
        public Vector2 GetCenter()
        {
            return new Vector2(m_Data.Position.X + m_Data.Width * 0.5f, m_Data.Position.Y + m_Data.Height * 0.5f);
        }
    }
}