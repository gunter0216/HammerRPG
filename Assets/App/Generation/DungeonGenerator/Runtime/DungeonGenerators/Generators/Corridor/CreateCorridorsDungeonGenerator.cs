using System;
using App.Common.Algorithms.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SquarePartition;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridor
{
    public class CreateCorridorsDungeonGenerator : IDungeonGenerator
    {
        private const int _corridorSize = 3;
        
        public CreateCorridorsDungeonGenerator()
        {
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            CreateCorridors(generation);

            return Optional<DungeonGeneration>.Success(generation);
        }

        private void CreateCorridors(DungeonGeneration generation)
        {
            var dungeon = generation.DungeonGenerationResult;
            var startRoom = dungeon.GenerationData.GenerationRooms.StartGenerationRoom;
            CreateCorridor(startRoom, null);
        }

        private void CreateCorridor(DungeonGenerationRoom room1, DungeonGenerationRoom prevRoom)
        {
            foreach (var connection in room1.Connections)
            {
                var room2 = connection.GenerationRoom;
                if (prevRoom != null && prevRoom == room2)
                {
                    continue;
                }

                var side = connection.Side;
                SquareArea area = null;
                if (side == RoomConnectSide.Right)
                {
                    var width = room2.Left - room1.Right;
                    var height = _corridorSize;
                    var positionY = (int)((room1.Center.Y + room2.Center.Y) / 2) - (height / 2);
                    area = new SquareArea(new Vector2Int(room1.Right, positionY), new Vector2Int(width, height));
                }
                else if (side == RoomConnectSide.Left)
                {
                    var width = room1.Left - room2.Right;
                    var height = _corridorSize;
                    var positionY = (int)((room1.Center.Y + room2.Center.Y) / 2) - (height / 2);
                    area = new SquareArea(new Vector2Int(room2.Right, positionY), new Vector2Int(width, height));
                }
                else if (side == RoomConnectSide.Top)
                {
                    var height = room2.Bottom - room1.Top;
                    var width = _corridorSize;
                    var positionX = (int)((room1.Center.X + room2.Center.X) / 2) - (width / 2);
                    area = new SquareArea(new Vector2Int(positionX, room1.Top), new Vector2Int(width, height));
                }
                else if (side == RoomConnectSide.Bottom)
                {
                    var height = room1.Bottom - room2.Top;
                    var width = _corridorSize;
                    var positionX = (int)((room1.Center.X + room2.Center.X) / 2) - (width / 2);
                    area = new SquareArea(new Vector2Int(positionX, room2.Top), new Vector2Int(width, height));
                }

                room2.Corridor = area;

                CreateCorridor(room2, room1);
            }
        }

        public string GetName()
        {
            return "Create Corridors";
        }
    }
}