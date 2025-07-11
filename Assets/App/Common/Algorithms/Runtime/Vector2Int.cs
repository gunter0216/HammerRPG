using System;

namespace App.Common.Algorithms.Runtime
{
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        private int m_X;
        private int m_Y;

        public int x
        {
            get => m_X;
            set => m_X = value;
        }

        public int y
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
            => new Vector2Int(a.x + b.x, a.y + b.y);

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
            => new Vector2Int(a.x - b.x, a.y - b.y);

        public static Vector2Int operator -(Vector2Int a)
            => new Vector2Int(-a.x, -a.y);

        public static Vector2Int operator *(Vector2Int a, int d)
            => new Vector2Int(a.x * d, a.y * d);

        public static Vector2Int operator *(int d, Vector2Int a)
            => a * d;

        public static Vector2Int operator /(Vector2Int a, int d)
            => new Vector2Int(a.x / d, a.y / d);

        public static bool operator ==(Vector2Int a, Vector2Int b)
            => a.x == b.x && a.y == b.y;

        public static bool operator !=(Vector2Int a, Vector2Int b)
            => !(a == b);

        public bool Equals(Vector2Int other)
            => this == other;

        public override bool Equals(object obj)
            => obj is Vector2Int other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(x, y);

        public override string ToString()
            => $"({x}, {y})";

        public int SqrMagnitude => x * x + y * y;

        public static int Distance(Vector2Int a, Vector2Int b)
            => (int)Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));

        public static Vector2Int Zero => new Vector2Int(0, 0);
    }
}