using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Algorithms.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.Matrix;

namespace App.Generation.DungeonGenerator.Runtime.Rooms
{
    public class DungeonGenerationRoom
    {
        private readonly int m_UID;
        private Vector2Int m_Position;
        private Vector2Int m_Size;
        private readonly List<DungeonKeyData> m_ContainsDoorKeys;
        private readonly List<RoomConnection> m_Connections;
        private List<GeneraitonTile> m_Tiles;
        private DungeonKeyData m_RequiredKey;
        private bool m_IsMainPath;
        private Matrix<GeneraitonTile> m_Matrix;

        public int Col => m_Position.X;
        public int Row => m_Position.Y;
        
        public int Width => m_Size.X;
        public int Height => m_Size.Y;

        public int Right => m_Position.X + m_Size.X;
        public int Left => m_Position.X;
        public int Top => m_Position.Y + m_Size.Y;
        public int Bottom => m_Position.Y;

        public int UID => m_UID;

        public Vector2 Center => GetCenter();

        public Vector2Int Position
        {
            get => m_Position;
            set => m_Position = value;
        }

        public Vector2Int Size
        {
            get => m_Size;
            set => m_Size = value;
        }

        public IReadOnlyList<DungeonKeyData> ContainsDoorKeys => m_ContainsDoorKeys;
        public IReadOnlyList<RoomConnection> Connections => m_Connections;

        public DungeonKeyData RequiredKey
        {
            get => m_RequiredKey;
            set => m_RequiredKey = value;
        }

        public bool IsMainPath
        {
            get => m_IsMainPath;
            set => m_IsMainPath = value;
        }

        public List<GeneraitonTile> Tiles
        {
            get => m_Tiles;
            set => m_Tiles = value;
        }

        public Matrix<GeneraitonTile> Matrix
        {
            get => m_Matrix;
            set => m_Matrix = value;
        }

        public DungeonGenerationRoom(int uid, Vector2Int position, Vector2Int size)
        {
            m_Size = size;
            m_UID = uid;
            m_Position = position;
            m_ContainsDoorKeys = new List<DungeonKeyData>();
            m_Connections = new List<RoomConnection>();
            Tiles = new List<GeneraitonTile>();
        }

        public bool AddDoorKey(DungeonKeyData dungeonKeyData)
        {
            if (!m_ContainsDoorKeys.Contains(dungeonKeyData))
            {
                m_ContainsDoorKeys.Add(dungeonKeyData);
                return true;
            }

            return false;
        }
        
        public bool AddConnection(RoomConnection connection)
        {
            if (!m_Connections.Contains(connection))
            {
                m_Connections.Add(connection);
                return true;
            }

            return false;
        }
        
        public Vector2 GetCenter()
        {
            return new Vector2(m_Position.X + Width * 0.5f, m_Position.Y + Height * 0.5f);
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
            m_Position += value;
        }

        public void DecreaseHeight(int value)
        {
            m_Size.Y -= value;
        }

        public void DecreaseWidth(int value)
        {
            m_Size.X -= value;
        }

        public void IncreaseHeight(int value)
        {
            m_Size.Y += value;
        }

        public void IncreaseWidth(int value)
        {
            m_Size.X += value;
        }

        public Vector2Int LocalToWorld(int x, int y) 
        {
            return new Vector2Int(m_Position.X + x, m_Position.Y + Height - 1 - y);
        }
        
        public Vector2Int LocalToWorld(Vector2Int localPosition)
        {
            return LocalToWorld(localPosition.X, localPosition.Y);
        }
        
        public Vector2Int WorldToLocal(int x, int y) 
        {
            return new Vector2Int(x - m_Position.X, m_Position.Y - y + Height - 1);
        }

        public Vector2Int WorldToLocal(Vector2Int worldPosition)
        {
            return WorldToLocal(worldPosition.X, worldPosition.Y);
        }

        public IReadOnlyList<RoomConnection> GetConnectionsExclude(DungeonGenerationRoom generationRoom)
        {
            if (generationRoom == null)
            {
                return m_Connections;
            }

            return m_Connections.Where(x => x.GenerationRoom != generationRoom).ToArray();
        }

        public override int GetHashCode()
        {
            return m_UID;
        }

        public override string ToString()
        {
            return $"Room [ UID: {m_UID}, Center: {GetCenter()}, Size: {m_Size}, Position {m_Position}]";
        }
    }
}