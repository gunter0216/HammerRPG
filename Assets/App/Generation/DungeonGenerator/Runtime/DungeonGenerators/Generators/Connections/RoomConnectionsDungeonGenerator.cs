using System;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Connections
{
    public class RoomConnectionsDungeonGenerator : IDungeonGenerator
    {
        public RoomConnectionsDungeonGenerator()
        {
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            if (!generation.TryGetCash<SpanningTreeGenerationCash>(out var cash))
            {
                return Optional<DungeonGeneration>.Fail();
            }

            CreateCorridors(cash);

            return Optional<DungeonGeneration>.Success(generation);
        }

        private void CreateCorridors(SpanningTreeGenerationCash spanningTree)
        {
            foreach (var edge in spanningTree.Tree)
            {
                SetConnections(edge.Room1, edge.Room2);
            }
        }

        private void SetConnections(DungeonGenerationRoom room1, DungeonGenerationRoom room2)
        {
            var center1 = room1.GetCenter();
            var center2 = room2.GetCenter();

            float deltaX = Math.Abs(center2.X - center1.X);
            float deltaY = Math.Abs(center2.Y - center1.Y);

            bool isHorizontal = deltaX > deltaY;
            
            if (isHorizontal)
            {
                if (room1.Left < room2.Left)
                {
                    room1.AddConnection(new RoomConnection(room2, RoomConnectSide.Right));
                    room2.AddConnection(new RoomConnection(room1, RoomConnectSide.Left));
                }
                else
                {
                    room1.AddConnection(new RoomConnection(room2, RoomConnectSide.Left));
                    room2.AddConnection(new RoomConnection(room1, RoomConnectSide.Right));
                }
            }
            else
            {
                if (room1.Bottom < room2.Bottom)
                {
                    room1.AddConnection(new RoomConnection(room2, RoomConnectSide.Top));
                    room2.AddConnection(new RoomConnection(room1, RoomConnectSide.Bottom));
                }
                else
                {
                    room1.AddConnection(new RoomConnection(room2, RoomConnectSide.Bottom));
                    room2.AddConnection(new RoomConnection(room1, RoomConnectSide.Top));
                }
            }
        }

        public string GetName()
        {
            return "Create room connections";
        }
    }
}