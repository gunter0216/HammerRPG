using System;
using App.Common.Algorithms.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.Rooms
{
    public class DungeonRoomData
    {
        private readonly int m_UID;
        private Vector2Int m_Position;
        private Vector2Int m_Size;
        
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

        public DungeonRoomData(int uid, Vector2Int position, Vector2Int size)
        {
            this.m_Size = size;
            m_UID = uid;
            this.m_Position = position;
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
        
        public bool Intersects(DungeonRoomData second)
        {
            return IsOverlapping(second);
        }
        
        public bool IsOverlapping(DungeonRoomData second)
        {
            return Left < second.Right && Right > second.Left &&
                   Top > second.Bottom && Bottom < second.Top;
        }
        
        public float GetArea()
        {
            return Width * Height;
        }
        
        // public bool IsOverlapping(DungeonRoomData second)
        // {
        //     return Left <= second.Right && Right >= second.Left &&
        //            Top >= second.Bottom && Bottom <= second.Top;
        // }

        public override string ToString()
        {
            return $"Room [ Position: {GetCenter()}, Size: {m_Size}, Left {m_Position}]";
        }
    }
}