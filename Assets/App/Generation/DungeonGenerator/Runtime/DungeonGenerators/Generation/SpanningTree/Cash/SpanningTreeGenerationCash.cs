using System.Collections.Generic;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash
{
    public class SpanningTreeGenerationCash : IGenerationCash
    {
        private readonly List<WeightRoomPair> m_Tree;

        public SpanningTreeGenerationCash(List<WeightRoomPair> tree)
        {
            m_Tree = tree;
        }

        public List<WeightRoomPair> Tree => m_Tree;
    }
}