using System;
using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Config;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding
{
    public class SelectBorderingRoomsDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var dungeon = generation.DungeonGenerationResult;
            if (!generation.TryGetConfig<SelectBorderingRoomsGenerationConfig>(out var config))
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            var borderingRooms = GetBorderingRooms(dungeon, config);
            generation.AddCash(new BorderingRoomsGenerationCash(borderingRooms));
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        private HashSet<int> GetBorderingRooms(DungeonGenerationResult dungeonGenerationResult, SelectBorderingRoomsGenerationConfig config)
        {
            var minCorridorSize = config.MinCorridorSize;
            var roomsCount = dungeonGenerationResult.GenerationData.GenerationRooms.Rooms.Count;
            var rooms = dungeonGenerationResult.GenerationData.GenerationRooms.Rooms;

            var borderingRooms = new HashSet<int>(rooms.Count);
	        
            for (int i = 0; i < roomsCount; ++i)
            {
                var room = rooms[i];
		        
                for (int j = i + 1; j < roomsCount; ++j)
                {
                    var other = rooms[j];
			        
                    var distance = room.GetCenter() - other.GetCenter();
                    var roomDistX = Math.Abs(distance.X);
                    var roomDistY = Math.Abs(distance.Y);
			        
                    var minCorridorSizeSpaceX = room.Width / 2 + other.Width / 2 + minCorridorSize;
                    var minCorridorSizeSpaceY = room.Height / 2 + other.Height / 2 + minCorridorSize;
			        
                    var isCorridorFlat = roomDistX > roomDistY;
			        
                    if (isCorridorFlat && roomDistX < minCorridorSizeSpaceX ||
                        !isCorridorFlat && roomDistY < minCorridorSizeSpaceY)
                    {
                        borderingRooms.Add(room.UID);
                        break;
                    }
                }
            }

            return borderingRooms;
        }

        public string GetName()
        {
            return "Select Bordering Rooms";
        }
    }
}