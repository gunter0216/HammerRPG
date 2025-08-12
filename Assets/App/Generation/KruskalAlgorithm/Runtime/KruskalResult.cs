using System.Collections.Generic;
using Assets.App.Common.Algorithms.Runtime;

namespace App.Generation.KruskalAlgorithm.Runtime
{
    public class KruskalResult
    {
        public Dictionary<int, Vector2> IndexToPoint { internal set; get; }
        public List<Edge> MinimumSpanningTree { get; }
        public double TotalWeight { get; }
        public bool IsConnected { get; }

        public KruskalResult(List<Edge> mst, double totalWeight, bool isConnected)
        {
            MinimumSpanningTree = mst;
            TotalWeight = totalWeight;
            IsConnected = isConnected;
        }
    }
}