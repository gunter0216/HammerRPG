using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Cash;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding
{
    public class DiscardSmallRoomsDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var dungeon = generation.Dungeon;
            if (!generation.TryGetCash<SmallRoomsGenerationCash>(out var cash))
            {
                return Optional<DungeonGeneration>.Fail();
            } 
            
            dungeon.DiscardRooms(cash.SmallRooms);
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        public string GetName()
        {
            return "Discard Small Rooms";
        }
    }
}