using System.Collections.Generic;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Config;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding
{
    public class SelectSmallRoomsDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var dungeon = generation.DungeonGenerationResult;
            if (!generation.TryGetConfig<SelectSmallRoomsGenerationConfig>(out var config))
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            var smallRooms = GetSmallRooms(dungeon, config);
            generation.AddCash(new SmallRoomsGenerationCash(smallRooms));

            return Optional<DungeonGeneration>.Success(generation);
        }

        public HashSet<int> GetSmallRooms(DungeonGenerationResult dungeonGenerationResult, SelectSmallRoomsGenerationConfig config)
        {
            var rooms = dungeonGenerationResult.GenerationData.GenerationRooms.Rooms;
            var smallRooms = new HashSet<int>(rooms.Count);
            for (int i = 0; i < rooms.Count; ++i)
            {
                var room = rooms[i];

                if (room.Width < config.WidthRoomThreshold ||
                    room.Height < config.HeightRoomThreshold)
                {
                    smallRooms.Add(room.UID);
                }
            }

            return smallRooms;
        }

        public string GetName()
        {
            return "Select Small Rooms";
        }
    }
}