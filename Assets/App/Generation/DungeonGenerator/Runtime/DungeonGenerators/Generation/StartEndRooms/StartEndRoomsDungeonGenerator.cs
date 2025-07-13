using System.Collections.Generic;
using App.Common.Utility.Runtime;
using App.Generation.BFS.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndRooms
{
    public class StartEndRoomsDungeonGenerator : IDungeonGenerator
    {
        private readonly BFSAlgorithm m_BfsAlgorithm;

        public StartEndRoomsDungeonGenerator()
        {
            m_BfsAlgorithm = new BFSAlgorithm();
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            if (!generation.TryGetCash<SpanningTreeGenerationCash>(out var cash))
            {
                return Optional<DungeonGeneration>.Fail();
            }

            var tree = cash.Tree;
            var edges = new List<(int, int)>(tree.Count);
            var indexToRoom = new Dictionary<int, DungeonRoomData>();
            var UIDToIndex = new Dictionary<int, int>();
            int index = 0;
            for (int i = 0; i < tree.Count; ++i)
            {
                var edge = tree[i];
                if (!UIDToIndex.TryGetValue(edge.Room1.UID, out var index1))
                {
                    UIDToIndex.Add(edge.Room1.UID, index);
                    indexToRoom.Add(index, edge.Room1);
                    index1 = index++;
                }
                
                if (!UIDToIndex.TryGetValue(edge.Room2.UID, out var index2))
                {
                    UIDToIndex.Add(edge.Room2.UID, index);
                    indexToRoom.Add(index, edge.Room2);
                    index2 = index++;
                }
                
                edges.Add((index1, index2));
            }

            var result = m_BfsAlgorithm.FindFarthestNodes(edges, index);
            var source = indexToRoom[result.Item1];
            var target = indexToRoom[result.Item2];

            generation.Dungeon.Data.RoomsData.StartRoom = source;
            generation.Dungeon.Data.RoomsData.EndRoom = target;
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        public string GetName()
        {
            return "Select Start and End rooms";
        }
    }
}