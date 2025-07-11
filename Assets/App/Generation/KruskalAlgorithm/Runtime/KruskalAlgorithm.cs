using System;
using System.Collections.Generic;
using System.Linq;
using App.Generation.DelaunayTriangulation.Runtime;

// Основной класс для алгоритма Краскала
namespace App.Generation.KruskalAlgorithm.Runtime
{
    public class KruskalAlgorithm
    {
        public (KruskalResult result, Dictionary<int, Point> indexToPoint) FindMinimumSpanningTree(List<Triangle> triangles)
        {
            if (triangles == null || triangles.Count == 0)
                return default;

            Console.WriteLine("=== Построение графа из треугольников ===");

            // Собираем все уникальные точки
            var uniquePoints = new HashSet<Point>();
            foreach (var triangle in triangles)
            {
                uniquePoints.Add(triangle.A);
                uniquePoints.Add(triangle.B);
                uniquePoints.Add(triangle.C);
            }

            var pointList = uniquePoints.ToList();
            var pointToIndex = new Dictionary<Point, int>();
            var indexToPoint = new Dictionary<int, Point>();

            for (int i = 0; i < pointList.Count; i++)
            {
                pointToIndex[pointList[i]] = i;
                indexToPoint[i] = pointList[i];
            }

            Console.WriteLine($"Найдено {pointList.Count} уникальных точек");

            // Создаём граф
            var graph = new WeightedGraph(pointList.Count);
            var addedEdges = new HashSet<(int, int)>();

            foreach (var triangle in triangles)
            {
                // Получаем индексы точек
                int indexA = pointToIndex[triangle.A];
                int indexB = pointToIndex[triangle.B];
                int indexC = pointToIndex[triangle.C];

                // Добавляем рёбра треугольника (избегаем дубликатов)
                var edges = new[]
                {
                    (Math.Min(indexA, indexB), Math.Max(indexA, indexB), CalculateDistance(triangle.A, triangle.B)),
                    (Math.Min(indexB, indexC), Math.Max(indexB, indexC), CalculateDistance(triangle.B, triangle.C)),
                    (Math.Min(indexC, indexA), Math.Max(indexC, indexA), CalculateDistance(triangle.C, triangle.A))
                };

                foreach (var (src, dst, weight) in edges)
                {
                    if (!addedEdges.Contains((src, dst)))
                    {
                        graph.AddEdge(src, dst, weight);
                        addedEdges.Add((src, dst));
                    }
                }
            }

            Console.WriteLine($"Добавлено {graph.Edges.Count} уникальных рёбер");

            // Выполняем алгоритм Краскала
            var result = FindMinimumSpanningTree(graph);

            // Если нужно, выводим результат с координатами точек
            if (result.IsConnected)
            {
                Console.WriteLine("\nРёбра MST с координатами:");
                foreach (var edge in result.MinimumSpanningTree)
                {
                    var pointA = pointList[edge.Source];
                    var pointB = pointList[edge.Destination];
                    Console.WriteLine($"  {pointA} -> {pointB}, вес: {edge.Weight:F2}");
                }
            }

            return (result, indexToPoint);
        }
        
        private double CalculateDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        // Главный метод - нахождение минимального остовного дерева
        public KruskalResult FindMinimumSpanningTree(WeightedGraph graph)
        {
            if (graph.VertexCount == 0)
                return new KruskalResult(new List<Edge>(), 0, true);

            // Проверяем связность графа
            if (!graph.IsConnected())
            {
                Console.WriteLine("Граф не связен - невозможно построить остовное дерево");
                return new KruskalResult(new List<Edge>(), 0, false);
            }

            var mst = new List<Edge>();
            var unionFind = new UnionFind(graph.VertexCount);
            double totalWeight = 0;

            // Сортируем рёбра по весу
            var sortedEdges = graph.Edges.OrderBy(e => e.Weight).ToList();

            Console.WriteLine("Обработка рёбер в порядке возрастания веса:");

            foreach (var edge in sortedEdges)
            {
                // Проверяем, создаёт ли ребро цикл
                if (!unionFind.Connected(edge.Source, edge.Destination))
                {
                    // Добавляем ребро в MST
                    mst.Add(edge);
                    totalWeight += edge.Weight;
                    unionFind.Union(edge.Source, edge.Destination);

                    Console.WriteLine($"✓ Добавлено: {edge}");

                    // Если у нас уже есть n-1 рёбер, остовное дерево готово
                    if (mst.Count == graph.VertexCount - 1)
                        break;
                }
                else
                {
                    Console.WriteLine($"✗ Пропущено (создаёт цикл): {edge}");
                }
            }

            return new KruskalResult(mst, totalWeight, true);
        }

        // Проверка корректности остовного дерева
        public bool ValidateSpanningTree(WeightedGraph originalGraph, List<Edge> spanningTree)
        {
            // Проверяем количество рёбер
            if (spanningTree.Count != originalGraph.VertexCount - 1)
            {
                Console.WriteLine(
                    $"Неверное количество рёбер: {spanningTree.Count}, ожидалось: {originalGraph.VertexCount - 1}");
                return false;
            }

            // Проверяем, что все рёбра принадлежат исходному графу
            foreach (var edge in spanningTree)
            {
                bool found = originalGraph.Edges.Any(e =>
                    (e.Source == edge.Source && e.Destination == edge.Destination &&
                     Math.Abs(e.Weight - edge.Weight) < 1e-9) ||
                    (e.Source == edge.Destination && e.Destination == edge.Source &&
                     Math.Abs(e.Weight - edge.Weight) < 1e-9));

                if (!found)
                {
                    Console.WriteLine($"Ребро {edge} не найдено в исходном графе");
                    return false;
                }
            }

            // Проверяем связность получившегося дерева
            var treeGraph = new WeightedGraph(originalGraph.VertexCount);
            foreach (var edge in spanningTree)
            {
                treeGraph.AddEdge(edge.Source, edge.Destination, edge.Weight);
            }

            if (!treeGraph.IsConnected())
            {
                Console.WriteLine("Остовное дерево не связно");
                return false;
            }

            return true;
        }
    }
}

// Пример использования
// public class Program
// {
//     public static void Main()
//     {
//         Console.WriteLine("=== Алгоритм Краскала для поиска минимального остовного дерева ===\n");
//
//         // Создаём тестовый граф
//         var graph = CreateSampleGraph();
//         
//         Console.WriteLine("Исходный граф:");
//         PrintGraph(graph);
//
//         // Находим минимальное остовное дерево
//         Console.WriteLine("\n=== Выполнение алгоритма Краскала ===");
//         var result = KruskalAlgorithm.FindMinimumSpanningTree(graph);
//
//         if (result.IsConnected)
//         {
//             Console.WriteLine($"\n=== Результат ===");
//             Console.WriteLine($"Минимальное остовное дерево найдено!");
//             Console.WriteLine($"Общий вес: {result.TotalWeight:F2}");
//             Console.WriteLine($"Количество рёбер: {result.MinimumSpanningTree.Count}");
//             
//             Console.WriteLine("\nРёбра минимального остовного дерева:");
//             foreach (var edge in result.MinimumSpanningTree)
//             {
//                 Console.WriteLine($"  {edge}");
//             }
//
//             // Проверяем корректность
//             Console.WriteLine("\n=== Проверка корректности ===");
//             bool isValid = KruskalAlgorithm.ValidateSpanningTree(graph, result.MinimumSpanningTree);
//             Console.WriteLine($"Остовное дерево корректно: {isValid}");
//         }
//
//         // Демонстрация на несвязном графе
//         Console.WriteLine("\n=== Тест на несвязном графе ===");
//         var disconnectedGraph = CreateDisconnectedGraph();
//         var disconnectedResult = KruskalAlgorithm.FindMinimumSpanningTree(disconnectedGraph);
//     }
//
//     private static WeightedGraph CreateSampleGraph()
//     {
//         // Создаём граф с 6 вершинами
//         var graph = new WeightedGraph(6);
//         
//         // Добавляем рёбра (создаём связный граф)
//         graph.AddEdge(0, 1, 4);
//         graph.AddEdge(0, 2, 3);
//         graph.AddEdge(1, 2, 1);
//         graph.AddEdge(1, 3, 2);
//         graph.AddEdge(2, 3, 4);
//         graph.AddEdge(3, 4, 2);
//         graph.AddEdge(3, 5, 6);
//         graph.AddEdge(4, 5, 3);
//         graph.AddEdge(1, 4, 5);
//         graph.AddEdge(2, 5, 8);
//
//         return graph;
//     }
//
//     private static WeightedGraph CreateDisconnectedGraph()
//     {
//         var graph = new WeightedGraph(5);
//         
//         // Создаём два несвязанных компонента
//         graph.AddEdge(0, 1, 1);
//         graph.AddEdge(1, 2, 2);
//         // Вершины 3 и 4 образуют отдельный компонент
//         graph.AddEdge(3, 4, 3);
//
//         return graph;
//     }
//
//     private static void PrintGraph(WeightedGraph graph)
//     {
//         Console.WriteLine($"Вершин: {graph.VertexCount}");
//         Console.WriteLine($"Рёбер: {graph.Edges.Count}");
//         Console.WriteLine("Рёбра:");
//         
//         foreach (var edge in graph.Edges.OrderBy(e => e.Weight))
//         {
//             Console.WriteLine($"  {edge}");
//         }
//     }
// }