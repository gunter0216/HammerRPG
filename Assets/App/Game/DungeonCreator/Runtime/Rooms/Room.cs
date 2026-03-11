using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Game.DungeonCreator.Runtime.Tiles;
using App.Game.GameManagers.External.Room;
using App.Generation.DungeonCreator.Runtime.Chest;

namespace App.Game.DungeonCreator.Runtime.Rooms
{
    public class Room
    {
        private readonly RoomData _data;
        private List<Tile> _tiles;
        private List<Door> _doors;
        private List<Chest> _chests;

        public Room(RoomData data)
        {
            _data = data;
        }

        public RoomData Data => _data;

        public List<Tile> Tiles
        {
            get => _tiles;
            set => _tiles = value;
        }
        
        public int Width => _data.Width;
        public int Height => _data.Height;
        public Vector2Int Position => _data.Position;
        public Vector2Int Size => _data.Size;
        public int Col => Position.X;
        public int Row => Position.Y;

        public List<Door> Doors
        {
            get => _doors;
            set => _doors = value;
        }

        public List<Chest> Chests
        {
            get => _chests;
            set => _chests = value;
        }

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
        
        public Vector2Int GetLocalCenter()
        {
            return new Vector2Int(Width / 2, Height / 2);
        }
        
        public Vector2 GetCenter()
        {
            return new Vector2(_data.Position.X + _data.Width * 0.5f, _data.Position.Y + _data.Height * 0.5f);
        }
    }
}