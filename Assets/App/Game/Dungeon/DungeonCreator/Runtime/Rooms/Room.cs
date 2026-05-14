using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Doors;
using App.Game.Dungeon.DungeonCreator.Runtime.Tiles;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Rooms
{
    public class Room
    {
        private readonly RoomData _data;
        private readonly RoomGenerationConfig _config;
        private readonly RoomConfigVariant _variant;
        private List<Tile> _tiles;
        private List<Door> _doors;
        private List<Chest.Chest> _chests;

        public Room(RoomData data, RoomGenerationConfig config, RoomConfigVariant variant)
        {
            _data = data;
            _config = config;
            _variant = variant;
        }

        public RoomData Data => _data;

        public List<Tile> Tiles
        {
            get => _tiles;
            set => _tiles = value;
        }
        
        public int Width => Config.Size.X;
        public int Height => Config.Size.Y;
        public Vector2Int Position => _data.Position;
        public Vector2Int Size => Config.Size;
        public int Col => Position.X;
        public int Row => Position.Y;

        public List<Door> Doors
        {
            get => _doors;
            set => _doors = value;
        }

        public List<Chest.Chest> Chests
        {
            get => _chests;
            set => _chests = value;
        }

        public RoomGenerationConfig Config => _config;

        public RoomConfigVariant Variant => _variant;

        public Vector2Int LocalToWorld(int x, int y) 
        {
            return new Vector2Int(Position.X + x, Position.Y + y);
        }
        
        public Vector2Int LocalToWorld(Vector2Int localPosition)
        {
            return LocalToWorld(localPosition.X, localPosition.Y);
        }
        
        public Vector2Int WorldToLocal(int x, int y) 
        {
            return new Vector2Int(x - Position.X, y - Position.Y);
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
            return new Vector2(_data.Position.X + Width * 0.5f, _data.Position.Y + Height * 0.5f);
        }
    }
}