using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common
{
    public class DungeonTreeCreator
    {
        public DungeonTreeResult Create(List<WeightRoomPair> weightTree)
        {
            var edges = new List<(int, int)>(weightTree.Count);
            var indexToRoom = new Dictionary<int, DungeonRoomData>();
            var UIDToIndex = new Dictionary<int, int>();
            int index = 0;
            for (int i = 0; i < weightTree.Count; ++i)
            {
                var edge = weightTree[i];
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

            return new DungeonTreeResult(indexToRoom, UIDToIndex, edges, index);
        }
    }
}