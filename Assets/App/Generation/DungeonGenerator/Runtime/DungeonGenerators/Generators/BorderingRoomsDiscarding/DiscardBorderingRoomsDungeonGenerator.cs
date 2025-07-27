using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Cash;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding
{
    public class DiscardBorderingRoomsDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var dungeon = generation.DungeonGenerationResult;
            if (!generation.TryGetCash<BorderingRoomsGenerationCash>(out var cash))
            {
                return Optional<DungeonGeneration>.Fail();
            } 
            
            dungeon.DiscardRooms(cash.Rooms);
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        public string GetName()
        {
            return "Discard Bordering Rooms";
        }
    }
}