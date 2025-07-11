namespace App.Generation.DelaunayTriangulation.Runtime
{
    public struct Edge
    {
        public Point Start { get; }
        public Point End { get; }

        public Edge(Point start, Point end)
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