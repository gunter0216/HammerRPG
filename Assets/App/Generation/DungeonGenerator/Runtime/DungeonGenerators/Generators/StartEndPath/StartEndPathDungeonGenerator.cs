using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DFS.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath
{
    public class StartEndPathDungeonGenerator : IDungeonGenerator
    {
        private readonly DFSAlgorithm m_DFSAlgorithm;
        private readonly DungeonTreeCreator m_TreeCreator;

        public StartEndPathDungeonGenerator()
        {
            m_DFSAlgorithm = new DFSAlgorithm();
            m_TreeCreator = new DungeonTreeCreator();
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            if (!generation.TryGetCash<SpanningTreeGenerationCash>(out var cash))
            {
                return Optional<DungeonGeneration>.Fail();
            }

            var roomsData = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = roomsData.StartGenerationRoom;
            var endRoom = roomsData.EndGenerationRoom;

            var treeResult = m_TreeCreator.Create(cash.Tree);
            
            var edges = treeResult.Edges;
            var indexToRoom = treeResult.IndexToRoom;
            var uidToIndex = treeResult.UIDToIndex;

            var start = uidToIndex[startRoom.UID];
            var end = uidToIndex[endRoom.UID];

            var pathResult = m_DFSAlgorithm.FindPath(edges, start, end);
            var path = new List<DungeonGenerationRoom>(pathResult.Count);
            foreach (var index in pathResult)
            {
                path.Add(indexToRoom[index]);
            }

            generation.AddCash(new StartEndPathGenerationCash(path));
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        public string GetName()
        {
            return "Create path from start to end room";
        }
    }
}