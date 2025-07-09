namespace App.Game.DelaunayTriangulation.Runtime
{
    public struct Point
    {
        public float X { get; }
        public float Y { get; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X:F2}, {Y:F2})";
    }
}