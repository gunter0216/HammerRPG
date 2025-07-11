using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation.Cash;

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
            if (!generation.TryGetCash<TriangulationGenerationCash>(out var cash))
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            var result = m_KruskalAlgorithm.FindMinimumSpanningTree(cash.Triangles);
            generation.AddCash(new SpanningTreeGenerationCash(result));
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        public string GetName()
        {
            return "Spanning Tree";
        }
    }
}