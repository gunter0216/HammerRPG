using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Algorithms.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Door;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Chest;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridor;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased;
using Vector2 = App.Common.Algorithms.Runtime.Vector2;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.Rooms
{
    public class DungeonGenerationRoom
    {
        private readonly int _UID;
        private Vector2Int _position;
        private Vector2Int _size;
        private readonly List<DungeonKeyData> _containsDoorKeys;
        private readonly List<RoomConnection> _connections;
        private readonly List<GenerationDoor> _doors;
        private readonly List<DungeonGenerationChest> _chests;
        private readonly Dictionary<Vector2Int, GeneraitonTile> _tiles;
        private readonly List<RectInt> _floors;
        private DungeonKeyData _requiredKey;
        private bool _isMainPath;
        private DungeonCorridor _corridor;
        private RoomConfigAsset _configAsset;
        private RoomConfigVariant _configVariant;
        private float _rotateEuler;
        private int _depth;

        public int Col => _position.X;
        public int Row => _position.Y;
        
        public int Width => _size.X;
        public int Height => _size.Y;

        public int Right => _position.X + _size.X;
        public int Left => _position.X;
        public int Top => _position.Y + _size.Y;
        public int Bottom => _position.Y;

        public int UID => _UID;

        public Vector2 Center => GetCenter();
        public Vector2 LocalCenter => GetLocalCenter();

        public Vector2Int Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector2Int Size
        {
            get => _size;
            set => _size = value;
        }

        public IReadOnlyList<DungeonKeyData> ContainsDoorKeys => _containsDoorKeys;
        public IReadOnlyList<RoomConnection> Connections => _connections;

        public DungeonKeyData RequiredKey
        {
            get => _requiredKey;
            set => _requiredKey = value;
        }

        public bool IsMainPath
        {
            get => _isMainPath;
            set => _isMainPath = value;
        }

        public IReadOnlyDictionary<Vector2Int, GeneraitonTile> Tiles => _tiles;

        public List<GenerationDoor> Doors => _doors;

        public DungeonCorridor Corridor
        {
            get => _corridor;
            set => _corridor = value;
        }

        public List<RectInt> Floors => _floors;

        public List<DungeonGenerationChest> Chests => _chests;

        public RoomConfigAsset ConfigAsset
        {
            get => _configAsset;
        }

        public void SetConfig(RoomConfigAsset config, RoomConfigVariant variant)
        {
            _configAsset = config;
            _configVariant = variant;
        }

        public float RotateEuler
        {
            get => _rotateEuler;
            set => _rotateEuler = value;
        }

        public int Depth
        {
            get => _depth;
            set => _depth = value;
        }

        public RoomConfigVariant ConfigVariant
        {
            get => _configVariant;
        }

        public DungeonGenerationRoom(int uid, Vector2Int position, Vector2Int size)
        {
            _size = size;
            _UID = uid;
            _position = position;
            _containsDoorKeys = new List<DungeonKeyData>();
            _connections = new List<RoomConnection>();
            _doors = new List<GenerationDoor>();
            _tiles = new Dictionary<Vector2Int, GeneraitonTile>();
            _floors = new List<RectInt>();
            _chests = new List<DungeonGenerationChest>();
        }

        public void AddFloor(RectInt floor)
        {
            Floors.Add(floor);
        }

        public void SetTile(Vector2Int position, DungeonTile id)
        {
            _tiles[position] = new GeneraitonTile(id);
        }
        
        public void SetTile(int x, int y, DungeonTile id)
        {
            _tiles[new Vector2Int(x, y)] = new GeneraitonTile(id);
        }

        public void RemoveTile(Vector2Int position)
        {
            _tiles.Remove(position);
        }

        public void RemoveTile(int x, int y)
        {
            RemoveTile(new Vector2Int(x, y));
        }
        
        public void SetTile(int x, int y, GeneraitonTile tile)
        {
            _tiles[new Vector2Int(x, y)] = tile;
        }
        
        public DungeonTile GetTile(int x, int y)
        {
            if (_tiles.TryGetValue(new Vector2Int(x, y), out var tile))
            {
                return tile.Id;
            }
            
            return DungeonTile.Empty;
        }

        public bool AddDoor(GenerationDoor door)
        {
            Doors.Add(door);
            return true;
        }
        
        public bool AddDoorKey(DungeonKeyData dungeonKeyData)
        {
            if (!_containsDoorKeys.Contains(dungeonKeyData))
            {
                _containsDoorKeys.Add(dungeonKeyData);
                return true;
            }

            return false;
        }
        
        public bool AddConnection(RoomConnection connection)
        {
            if (!_connections.Contains(connection))
            {
                _connections.Add(connection);
                return true;
            }

            return false;
        }
        
        public Vector2 GetCenter()
        {
            return new Vector2(_position.X + Width * 0.5f, _position.Y + Height * 0.5f);
        }
        
        public Vector2 GetLocalCenter()
        {
            return new Vector2(Width * 0.5f, Height * 0.5f);
        }
        
        public void SetCenter(Vector2 center)
        {
            Position = new Vector2Int(
                (int)Math.Round(center.X - Width * 0.5f), 
                (int)Math.Round(center.Y - Height * 0.5f)
            );
        }
        
        public Vector2Int GetCenterInt()
        {
            return new Vector2Int(Row + Height / 2, Col + Width / 2);
        }
        
        public bool Intersects(DungeonGenerationRoom second)
        {
            return IsOverlapping(second);
        }
        
        public bool IsOverlapping(DungeonGenerationRoom second)
        {
            return Left < second.Right && Right > second.Left &&
                   Top > second.Bottom && Bottom < second.Top;
        }
        
        public float GetArea()
        {
            return Width * Height;
        }

        public void Move(Vector2Int value)
        {
            _position += value;
        }

        public void DecreaseHeight(int value)
        {
            _size.Y -= value;
        }

        public void DecreaseWidth(int value)
        {
            _size.X -= value;
        }

        public void IncreaseHeight(int value)
        {
            _size.Y += value;
        }

        public void IncreaseWidth(int value)
        {
            _size.X += value;
        }

        public Vector2Int LocalToWorld(int x, int y) 
        {
            return new Vector2Int(_position.X + x, _position.Y + y);
            // return new Vector2Int(m_Position.X + x, m_Position.Y + Height - 1 - y);
        }
        
        public Vector2Int LocalToWorld(Vector2Int localPosition)
        {
            return LocalToWorld(localPosition.X, localPosition.Y);
        }
        
        public Vector2Int WorldToLocal(int x, int y) 
        {
            return new Vector2Int(x - _position.X, y - _position.Y);
            // return new Vector2Int(x - m_Position.X, m_Position.Y - y + Height - 1);
        }

        public Vector2Int WorldToLocal(Vector2Int worldPosition)
        {
            return WorldToLocal(worldPosition.X, worldPosition.Y);
        }

        public IReadOnlyList<RoomConnection> GetConnectionsExclude(DungeonGenerationRoom generationRoom)
        {
            if (generationRoom == null)
            {
                return _connections;
            }

            return _connections.Where(x => x.GenerationRoom != generationRoom).ToArray();
        }

        public override int GetHashCode()
        {
            return _UID;
        }

        public override string ToString()
        {
            return $"Room [ UID: {_UID}, Center: {GetCenter()}, Size: {_size}, Position {_position}]";
        }
    }
}