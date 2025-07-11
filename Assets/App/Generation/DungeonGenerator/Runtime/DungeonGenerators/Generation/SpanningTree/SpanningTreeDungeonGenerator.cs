using App.Common.Utility.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree
{
    public class SpanningTreeDungeonGenerator : IDungeonGenerator
    {
        private KruskalAlgorithm.Runtime.KruskalAlgorithm m_KruskalAlgorithm;

        public SpanningTreeDungeonGenerator()
        {
            m_KruskalAlgorithm = new KruskalAlgorithm.Runtime.KruskalAlgorithm();
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            // m_KruskalAlgorithm.FindMinimumSpanningTree();
            
            return Optional<DungeonGeneration>.Success(generation);
        }
        
        

        public string GetName()
        {
            return "Spanning Tree";
        }
    }
}