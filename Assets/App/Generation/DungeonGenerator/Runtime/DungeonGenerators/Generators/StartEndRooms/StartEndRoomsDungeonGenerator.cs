using App.Common.Utility.Runtime;
using App.Generation.BFS.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndRooms
{
    public class StartEndRoomsDungeonGenerator : IDungeonGenerator
    {
        private readonly BFSAlgorithm m_BfsAlgorithm;
        private readonly DungeonTreeCreator m_TreeCreator;

        public StartEndRoomsDungeonGenerator()
        {
            m_BfsAlgorithm = new BFSAlgorithm();
            m_TreeCreator = new DungeonTreeCreator();
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            if (!generation.TryGetCash<SpanningTreeGenerationCash>(out var cash))
            {
                return Optional<DungeonGeneration>.Fail();
            }

            var treeResult = m_TreeCreator.Create(cash.Tree);
            
            var edges = treeResult.Edges;
            var indexToRoom = treeResult.IndexToRoom;
            var vertices = treeResult.Vertices;

            var result = m_BfsAlgorithm.FindFarthestNodes(edges, vertices);
            var source = indexToRoom[result.Item1];
            var target = indexToRoom[result.Item2];

            generation.DungeonGenerationResult.GenerationData.GenerationRooms.StartGenerationRoom = source;
            generation.DungeonGenerationResult.GenerationData.GenerationRooms.EndGenerationRoom = target;
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        public string GetName()
        {
            return "Select Start and End rooms";
        }
    }
}