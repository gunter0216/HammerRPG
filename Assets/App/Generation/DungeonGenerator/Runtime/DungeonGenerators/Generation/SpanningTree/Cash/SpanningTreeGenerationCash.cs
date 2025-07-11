using App.Generation.KruskalAlgorithm.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash
{
    public class SpanningTreeGenerationCash : IGenerationCash
    {
        private readonly KruskalResult m_Result;

        public SpanningTreeGenerationCash(KruskalResult result)
        {
            m_Result = result;
        }

        public KruskalResult Result => m_Result;
    }
}