namespace App.Generation.KruskalAlgorithm.Runtime
{
    public struct Edge
    {
        public int Source { get; }
        public int Destination { get; }
        public double Weight { get; }

        public Edge(int source, int destination, double weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }

        // IComparable для сортировки рёбер по весу
        public int CompareTo(Edge other)
        {
            return Weight.CompareTo(other.Weight);
        }

        public override string ToString()
        {
            return $"({Source} -> {Destination}, вес: {Weight:F2})";
        }
    }
}