using System.Collections.Generic;
using App.Common.Algorithms.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash
{
    public class SpanningTreeGenerationCash : IGenerationCash
    {
        private readonly Dictionary<int, Vector2> m_IndexToPoint;
        private readonly List<WeightRoomPair> m_Tree;

        public SpanningTreeGenerationCash(Dictionary<int, Vector2> indexToPoint, List<WeightRoomPair> tree)
        {
            m_IndexToPoint = indexToPoint;
            m_Tree = tree;
        }

        public Dictionary<int, Vector2> IndexToPoint => m_IndexToPoint;

        public List<WeightRoomPair> Tree => m_Tree;
    }
}