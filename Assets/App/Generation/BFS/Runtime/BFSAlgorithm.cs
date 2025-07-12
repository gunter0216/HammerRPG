using System;
using System.Collections.Generic;

namespace App.Generation.BFS.Runtime
{
    public class BFSAlgorithm
    {
        // Метод BFS, возвращает (самая дальняя вершина, расстояние до неё)
        private (int, int) BFS(int start, List<int>[] graph)
        {
            int n = graph.Length;
            int[] dist = new int[n];
            for (int i = 0; i < n; i++)
                dist[i] = -1;

            Queue<int> queue = new Queue<int>();
            dist[start] = 0;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                foreach (int neighbor in graph[v])
                {
                    if (dist[neighbor] == -1)
                    {
                        dist[neighbor] = dist[v] + 1;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            int maxDist = 0;
            int farthestNode = start;
            for (int i = 0; i < n; i++)
            {
                if (dist[i] > maxDist)
                {
                    maxDist = dist[i];
                    farthestNode = i;
                }
            }

            return (farthestNode, maxDist);
        }

        // Версия, принимающая список смежности
        public (int, int, int) FindFarthestNodes(List<int>[] graph)
        {
            var (u, _) = BFS(0, graph);
            var (v, dist) = BFS(u, graph);
            return (u, v, dist);
        }

        // Перегрузка: принимает список рёбер и количество вершин
        public (int, int, int) FindFarthestNodes(List<(int, int)> edges, int nodeCount)
        {
            // Построение графа
            var graph = new List<int>[nodeCount];
            for (int i = 0; i < nodeCount; i++)
                graph[i] = new List<int>();

            foreach (var (u, v) in edges)
            {
                graph[u].Add(v);
                graph[v].Add(u); // неориентированный граф
            }

            return FindFarthestNodes(graph);
        }
    }
}