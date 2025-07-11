using System;

namespace App.Generation.DungeonGenerator.Runtime.Utility
{
    public struct LengthNode<T> : IComparable<LengthNode<T>>
    {
        public T Value;
        public float Length;

        public LengthNode(T value, float length)
        {
            Value = value;
            Length = length;
        }

        public int CompareTo(LengthNode<T> other)
        {
            return Length.CompareTo(other.Length);
        }
    }
}