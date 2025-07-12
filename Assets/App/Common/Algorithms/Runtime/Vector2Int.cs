using System;

namespace App.Common.Algorithms.Runtime
{
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        private int m_X;
        private int m_Y;

        public int X
        {
            get => m_X;
            set => m_X = value;
        }

        public int Y
        {
            get => m_Y;
            set => m_Y = value;
        }

        public Vector2Int(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
            => new Vector2Int(a.X + b.X, a.Y + b.Y);

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
            => new Vector2Int(a.X - b.X, a.Y - b.Y);

        public static Vector2Int operator -(Vector2Int a)
            => new Vector2Int(-a.X, -a.Y);

        public static Vector2Int operator *(Vector2Int a, int d)
            => new Vector2Int(a.X * d, a.Y * d);

        public static Vector2Int operator *(int d, Vector2Int a)
            => a * d;

        public static Vector2Int operator /(Vector2Int a, int d)
            => new Vector2Int(a.X / d, a.Y / d);
        
        public static Vector2Int operator /(Vector2Int a, float d)
            => new Vector2Int((int)(a.X / d), (int)(a.Y / d));

        public static bool operator ==(Vector2Int a, Vector2Int b)
            => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Vector2Int a, Vector2Int b)
            => !(a == b);

        public bool Equals(Vector2Int other)
            => this == other;

        public override bool Equals(object obj)
            => obj is Vector2Int other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(X, Y);

        public override string ToString()
            => $"({X}, {Y})";

        public int SqrMagnitude => X * X + Y * Y;

        public static int Distance(Vector2Int a, Vector2Int b)
            => (int)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

        public static Vector2Int Zero => new Vector2Int(0, 0);
        
        public Vector2 ToVector()
        {
            return new Vector2(m_X, m_Y);
        }
    }
}