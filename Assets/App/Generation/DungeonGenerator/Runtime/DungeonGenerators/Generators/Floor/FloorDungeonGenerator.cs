using App.Common.Algorithms.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Floor
{
    public class FloorDungeonGenerator : IDungeonGenerator
    {
        public FloorDungeonGenerator()
        {
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var roomsData = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var rooms = roomsData.Rooms;

            foreach (var room in rooms)
            {
                room.AddFloor(new RectInt(Vector2Int.Zero, room.Size));
                var corridor = room.Corridor;
                if (corridor != null)
                {
                    room.AddFloor(new RectInt(corridor.Area.Position, corridor.Area.Size));
                }
            }
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        public string GetName()
        {
            return "Create floors";
        }
    }
}