using System;
using System.Collections.Generic;
using System.Linq;

// Основной класс для триангуляции Делоне
namespace App.Game.DelaunayTriangulation.Runtime
{
    public class DelaunayTriangulation : ITriangulation
    {
        private readonly List<Triangle> triangles;

        public DelaunayTriangulation()
        {
            triangles = new List<Triangle>();
        }

        // Главный метод - выполнение триангуляции Делоне алгоритмом Бойера-Ватсона
        public List<Triangle> Triangulate(IEnumerable<Point> points)
        {
            var pointList = points.ToList();
            if (pointList.Count < 3)
                throw new ArgumentException("Нужно минимум 3 точки для триангуляции");

            triangles.Clear();

            // Создаём супертреугольник, который содержит все точки
            var superTriangle = CreateSuperTriangle(pointList);
            triangles.Add(superTriangle);

            // Добавляем каждую точку по очереди
            foreach (var point in pointList)
            {
                AddPoint(point);
            }

            // Удаляем треугольники, которые содержат вершины супертреугольника
            triangles.RemoveAll(t => t.ContainsVertex(superTriangle.A) || 
                                     t.ContainsVertex(superTriangle.B) || 
                                     t.ContainsVertex(superTriangle.C));

            return new List<Triangle>(triangles);
        }

        // Создание супертреугольника, содержащего все точки
        private Triangle CreateSuperTriangle(List<Point> points)
        {
            float minX = points.Min(p => p.X);
            float minY = points.Min(p => p.Y);
            float maxX = points.Max(p => p.X);
            float maxY = points.Max(p => p.Y);

            float dx = maxX - minX;
            float dy = maxY - minY;
            float deltaMax = Math.Max(dx, dy);
            float midX = (minX + maxX) / 2;
            float midY = (minY + maxY) / 2;

            // Создаём большой треугольник
            var p1 = new Point(midX - 20 * deltaMax, midY - deltaMax);
            var p2 = new Point(midX, midY + 20 * deltaMax);
            var p3 = new Point(midX + 20 * deltaMax, midY - deltaMax);

            return new Triangle(p1, p2, p3);
        }

        // Добавление точки в триангуляцию (основной шаг алгоритма Бойера-Ватсона)
        private void AddPoint(Point point)
        {
            var badTriangles = new List<Triangle>();
            var polygon = new List<Edge>();

            // Находим все треугольники, описанные окружности которых содержат новую точку
            foreach (var triangle in triangles)
            {
                if (triangle.CircumcircleContains(point))
                {
                    badTriangles.Add(triangle);
                }
            }

            // Находим границу полигональной дыры
            foreach (var triangle in badTriangles)
            {
                var edges = new[]
                {
                    new Edge(triangle.A, triangle.B),
                    new Edge(triangle.B, triangle.C),
                    new Edge(triangle.C, triangle.A)
                };

                foreach (var edge in edges)
                {
                    bool isShared = false;
                    foreach (var otherTriangle in badTriangles)
                    {
                        if (triangle.Equals(otherTriangle)) continue;
                    
                        var otherEdges = new[]
                        {
                            new Edge(otherTriangle.A, otherTriangle.B),
                            new Edge(otherTriangle.B, otherTriangle.C),
                            new Edge(otherTriangle.C, otherTriangle.A)
                        };

                        if (otherEdges.Any(e => e.Equals(edge)))
                        {
                            isShared = true;
                            break;
                        }
                    }

                    if (!isShared)
                    {
                        polygon.Add(edge);
                    }
                }
            }

            // Удаляем плохие треугольники
            foreach (var badTriangle in badTriangles)
            {
                triangles.Remove(badTriangle);
            }

            // Создаём новые треугольники из точек полигона и новой точки
            foreach (var edge in polygon)
            {
                triangles.Add(new Triangle(edge.Start, edge.End, point));
            }
        }

        // Получение всех рёбер триангуляции
        public List<Edge> GetEdges()
        {
            var edges = new List<Edge>();
            foreach (var triangle in triangles)
            {
                edges.Add(new Edge(triangle.A, triangle.B));
                edges.Add(new Edge(triangle.B, triangle.C));
                edges.Add(new Edge(triangle.C, triangle.A));
            }
            return edges;
        }
    }
}

// Пример использования
// public class Program
// {
//     public static void Main()
//     {
//         // Создаём набор тестовых точек
//         var points = new List<Point>
//         {
//             new Point(0, 0),
//             new Point(1, 0),
//             new Point(0, 1),
//             new Point(1, 1),
//             new Point(0.5, 0.5),
//             new Point(2, 0),
//             new Point(2, 1),
//             new Point(-1, 0),
//             new Point(-1, 1)
//         };
//
//         Console.WriteLine("Исходные точки:");
//         foreach (var point in points)
//         {
//             Console.WriteLine($"  {point}");
//         }
//
//         // Выполняем триангуляцию
//         var delaunay = new DelaunayTriangulation();
//         var triangles = delaunay.Triangulate(points);
//
//         Console.WriteLine($"\nПолучено треугольников: {triangles.Count}");
//         Console.WriteLine("\nТреугольники:");
//         for (int i = 0; i < triangles.Count; i++)
//         {
//             Console.WriteLine($"  {i + 1}: {triangles[i]}");
//         }
//
//         // Получаем рёбра
//         var edges = delaunay.GetEdges();
//         Console.WriteLine($"\nВсего рёбер: {edges.Count}");
//         
//         // Проверяем свойство Делоне (опционально)
//         Console.WriteLine("\nПроверка свойства Делоне...");
//         bool isValid = ValidateDelaunayProperty(triangles, points);
//         Console.WriteLine($"Триангуляция соответствует условию Делоне: {isValid}");
//     }
//
//     // Проверка свойства Делоне
//     private static bool ValidateDelaunayProperty(List<Triangle> triangles, List<Point> allPoints)
//     {
//         foreach (var triangle in triangles)
//         {
//             foreach (var point in allPoints)
//             {
//                 if (triangle.ContainsVertex(point)) continue;
//                 
//                 try
//                 {
//                     if (triangle.CircumcircleContains(point))
//                     {
//                         Console.WriteLine($"Нарушение условия Делоне: точка {point} внутри описанной окружности треугольника {triangle}");
//                         return false;
//                     }
//                 }
//                 catch (InvalidOperationException)
//                 {
//                     // Игнорируем вырожденные треугольники
//                     continue;
//                 }
//             }
//         }
//         return true;
//     }
// }