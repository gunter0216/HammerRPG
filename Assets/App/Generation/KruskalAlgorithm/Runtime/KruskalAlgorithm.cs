using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.Algorithms.Runtime;

// Основной класс для алгоритма Краскала
namespace App.Generation.KruskalAlgorithm.Runtime
{
    public class KruskalAlgorithm
    {
        public KruskalResult FindMinimumSpanningTree(List<Triangle> triangles)
        {
            if (triangles == null || triangles.Count == 0)
                return default;

            Console.WriteLine("=== Построение графа из треугольников ===");

            // Собираем все уникальные точки
            var uniquePoints = new HashSet<Vector2>();
            foreach (var triangle in triangles)
            {
                uniquePoints.Add(triangle.A);
                uniquePoints.Add(triangle.B);
                uniquePoints.Add(triangle.C);
            }

            var pointList = uniquePoints.ToList();
            var pointToIndex = new Dictionary<Vector2, int>();
            var indexToPoint = new Dictionary<int, Vector2>();

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
            result.Value.IndexToPoint = indexToPoint;

            return result.Value;
        }
        
        private double CalculateDistance(Vector2 p1, Vector2 p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        // Главный метод - нахождение минимального остовного дерева
        public Optional<KruskalResult> FindMinimumSpanningTree(WeightedGraph graph)
        {
            if (graph.VertexCount == 0)
                return Optional<KruskalResult>.Fail();

            // Проверяем связность графа
            if (!graph.IsConnected())
            {
                Console.WriteLine("Граф не связен - невозможно построить остовное дерево");
                return Optional<KruskalResult>.Fail();
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

            return Optional<KruskalResult>.Success(new KruskalResult(mst, totalWeight, true));
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