using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation.Cash;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree
{
    public class SpanningTreeDungeonGenerator : IDungeonGenerator
    {
        private readonly ILogger m_Logger;
        private readonly KruskalAlgorithm.Runtime.KruskalAlgorithm m_KruskalAlgorithm;

        public SpanningTreeDungeonGenerator(ILogger logger)
        {
            m_Logger = logger;
            m_KruskalAlgorithm = new KruskalAlgorithm.Runtime.KruskalAlgorithm();
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            if (!generation.TryGetCash<TriangulationGenerationCash>(out var triangulationCash))
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            var result = m_KruskalAlgorithm.FindMinimumSpanningTree(triangulationCash.Triangles);
            

            var edges = new List<WeightRoomPair>(result.MinimumSpanningTree.Count);
            foreach (var edge in result.MinimumSpanningTree)
            {
                var source = triangulationCash.PointToRoom[result.IndexToPoint[edge.Source]];
                var destination = triangulationCash.PointToRoom[result.IndexToPoint[edge.Destination]];
                edges.Add(new WeightRoomPair(source, destination, edge.Weight));
            }

            generation.AddCash(new SpanningTreeGenerationCash(tree: edges));
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        public string GetName()
        {
            return "Spanning Tree";
        }
    }
}