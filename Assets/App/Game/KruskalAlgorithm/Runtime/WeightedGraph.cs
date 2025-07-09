using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Game.KruskalAlgorithm.Runtime
{
    public class WeightedGraph
    {
        private int vertices;
        private List<Edge> edges;

        public int VertexCount => vertices;
        public List<Edge> Edges => new List<Edge>(edges);

        public WeightedGraph(int vertexCount)
        {
            vertices = vertexCount;
            edges = new List<Edge>();
        }

        // Добавление ребра в граф
        public void AddEdge(int source, int destination, double weight)
        {
            if (source < 0 || source >= vertices || destination < 0 || destination >= vertices)
                throw new ArgumentException("Неверный индекс вершины");

            edges.Add(new Edge(source, destination, weight));
        }

        // Добавление ребра с проверкой на дубликаты
        public void AddEdgeNoDuplicates(int source, int destination, double weight)
        {
            bool exists = edges.Any(e => 
                (e.Source == source && e.Destination == destination) ||
                (e.Source == destination && e.Destination == source));

            if (!exists)
            {
                AddEdge(source, destination, weight);
            }
        }

        // Получение списка всех соседей вершины
        public List<(int vertex, double weight)> GetNeighbors(int vertex)
        {
            var neighbors = new List<(int, double)>();
        
            foreach (var edge in edges)
            {
                if (edge.Source == vertex)
                    neighbors.Add((edge.Destination, edge.Weight));
                else if (edge.Destination == vertex)
                    neighbors.Add((edge.Source, edge.Weight));
            }
        
            return neighbors;
        }

        // Проверка связности графа (DFS)
        public bool IsConnected()
        {
            // return true;
            if (vertices == 0) return true;
            
            var visited = new bool[vertices];
            var stack = new Stack<int>();
            
            stack.Push(0);
            visited[0] = true;
            int visitedCount = 1;
            
            while (stack.Count > 0)
            {
                int current = stack.Pop();
            
                foreach (var (neighbor, _) in GetNeighbors(current))
                {
                    if (!visited[neighbor])
                    {
                        visited[neighbor] = true;
                        stack.Push(neighbor);
                        visitedCount++;
                    }
                }
            }
            
            return visitedCount == vertices;
        }
    }
}