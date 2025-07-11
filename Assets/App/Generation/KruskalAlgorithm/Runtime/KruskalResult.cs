using System.Collections.Generic;

namespace App.Generation.KruskalAlgorithm.Runtime
{
    public class KruskalResult
    {
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