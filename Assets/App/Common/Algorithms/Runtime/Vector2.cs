using System;

namespace App.Common.Algorithms.Runtime
{
    public struct Vector2 : IEquatable<Vector2>
    {
        private float m_X;
        private float m_Y;

        public float x
        {
            get => m_X;
            set => m_X = value;
        }

        public float y
        {
            get => m_Y;
            set => m_Y = value;
        }

        public Vector2(float x, float y)
        {
            m_X = x;
            m_Y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
            => new Vector2(a.x + b.x, a.y + b.y);
        
        public static Vector2 operator +(Vector2 a, Vector2Int b)
            => new Vector2(a.x + b.x, a.y + b.y);

        public static Vector2 operator -(Vector2 a, Vector2 b)
            => new Vector2(a.x - b.x, a.y - b.y);

        public static Vector2 operator -(Vector2 a)
            => new Vector2(-a.x, -a.y);

        public static Vector2 operator *(Vector2 a, float d)
            => new Vector2(a.x * d, a.y * d);

        public static Vector2 operator *(float d, Vector2 a)
            => a * d;

        public static Vector2 operator /(Vector2 a, float d)
            => new Vector2(a.x / d, a.y / d);

        public static bool operator ==(Vector2 a, Vector2 b)
            => Math.Abs(a.x - b.x) < 1e-6f && Math.Abs(a.y - b.y) < 1e-6f;

        public static bool operator !=(Vector2 a, Vector2 b)
            => !(a == b);

        public bool Equals(Vector2 other)
            => this == other;

        public override bool Equals(object obj)
            => obj is Vector2 other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(x, y);

        public override string ToString()
            => $"({x:F3}, {y:F3})";

        public float Magnitude => MathF.Sqrt(x * x + y * y);

        public float SqrMagnitude => x * x + y * y;

        public Vector2 Normalized
        {
            get
            {
                float mag = Magnitude;
                return mag > 1e-6f ? this / mag : Zero;
            }
        }

        public static Vector2 Zero => new Vector2(0f, 0f);

        public static float Distance(Vector2 a, Vector2 b)
            => (a - b).Magnitude;

        public static float Dot(Vector2 a, Vector2 b)
            => a.x * b.x + a.y * b.y;

        public static float Angle(Vector2 from, Vector2 to)
        {
            float dot = Dot(from.Normalized, to.Normalized);
            dot = Math.Clamp(dot, -1f, 1f);
            return MathF.Acos(dot) * (180f / MathF.PI);
        }
    }
}