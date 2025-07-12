using System;

namespace App.Common.Algorithms.Runtime
{
    public struct Triangle
    {
        public Vector2 A { get; }
        public Vector2 B { get; }
        public Vector2 C { get; }

        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
        }

        // Проверка, содержит ли описанная окружность треугольника данную точку
        public bool CircumcircleContains(Vector2 point)
        {
            var center = GetCircumcenter();
            var radius = GetCircumradius();
            var distance = Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2));
            return distance < radius + 1e-10; // небольшая погрешность для численной стабильности
        }

        // Вычисление центра описанной окружности
        public Vector2 GetCircumcenter()
        {
            float ax = A.X, ay = A.Y;
            float bx = B.X, by = B.Y;
            float cx = C.X, cy = C.Y;

            float d = 2 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));

            if (Math.Abs(d) < 1e-10)
                throw new InvalidOperationException("Точки коллинеарны");

            float ux = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) +
                        (cx * cx + cy * cy) * (ay - by)) / d;
            float uy = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) +
                        (cx * cx + cy * cy) * (bx - ax)) / d;

            return new Vector2(ux, uy);
        }

        // Вычисление радиуса описанной окружности
        public float GetCircumradius()
        {
            var center = GetCircumcenter();
            return (float)Math.Sqrt(Math.Pow(A.X - center.X, 2) + Math.Pow(A.Y - center.Y, 2));
        }

        // Проверка, содержит ли треугольник данную вершину
        public bool ContainsVertex(Vector2 vertex)
        {
            return (A.X == vertex.X && A.Y == vertex.Y) ||
                   (B.X == vertex.X && B.Y == vertex.Y) ||
                   (C.X == vertex.X && C.Y == vertex.Y);
        }

        public override string ToString() => $"Triangle({A}, {B}, {C})";
    }
}