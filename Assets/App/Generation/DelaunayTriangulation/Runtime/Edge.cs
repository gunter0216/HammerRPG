using Assets.App.Common.Algorithms.Runtime;

namespace App.Generation.DelaunayTriangulation.Runtime
{
    public struct Edge
    {
        public Vector2 Start { get; }
        public Vector2 End { get; }

        public Edge(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        // Проверка равенства рёбер (учитывает обе ориентации)
        public bool Equals(Edge other)
        {
            return (Start.X == other.Start.X && Start.Y == other.Start.Y &&
                    End.X == other.End.X && End.Y == other.End.Y) ||
                   (Start.X == other.End.X && Start.Y == other.End.Y &&
                    End.X == other.Start.X && End.Y == other.Start.Y);
        }

        public override string ToString() => $"Edge({Start} -> {End})";
    }
}