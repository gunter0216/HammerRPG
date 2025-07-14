using System;
using System.Collections.Generic;

namespace App.Generation.DFS.Runtime
{
    public class DFSAlgorithm
    {
        private readonly Dictionary<int, List<int>> m_AdjacencyList;

        public DFSAlgorithm()
        {
            m_AdjacencyList = new Dictionary<int, List<int>>();
        }

        // Добавление ребра в граф
        private void AddEdge(int from, int to)
        {
            if (!m_AdjacencyList.ContainsKey(from))
                m_AdjacencyList[from] = new List<int>();
            if (!m_AdjacencyList.ContainsKey(to))
                m_AdjacencyList[to] = new List<int>();

            m_AdjacencyList[from].Add(to);
            m_AdjacencyList[to].Add(from); // Для неориентированного графа
        }

        // Поиск пути с использованием DFS (поиск в глубину)
        public List<int> FindPath(List<ValueTuple<int, int>> edges, int start, int end)
        {
            // Очищаем граф и строим его заново из переданных рёбер
            m_AdjacencyList.Clear();

            foreach (var edge in edges)
            {
                AddEdge(edge.Item1, edge.Item2);
            }

            if (start == end)
                return new List<int> { start };

            if (!m_AdjacencyList.ContainsKey(start) || !m_AdjacencyList.ContainsKey(end))
                return new List<int>(); // Пустой список если вершины не существуют

            var visited = new HashSet<int>();
            var path = new List<int>();

            if (DFSRecursive(start, end, visited, path))
                return path;

            return new List<int>(); // Путь не найден
        }

        private bool DFSRecursive(int current, int target, HashSet<int> visited, List<int> path)
        {
            visited.Add(current);
            path.Add(current);

            if (current == target)
                return true;

            if (m_AdjacencyList.ContainsKey(current))
            {
                foreach (int neighbor in m_AdjacencyList[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        if (DFSRecursive(neighbor, target, visited, path))
                            return true;
                    }
                }
            }

            path.RemoveAt(path.Count - 1); // Backtrack
            return false;
        }
    }
}