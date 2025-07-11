using System;
using System.Collections.Generic;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Config;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding
{
    public class SelectBorderingRoomsDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var dungeon = generation.Dungeon;
            if (!generation.TryGetConfig<SelectBorderingRoomsGenerationConfig>(out var config))
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            var borderingRooms = GetBorderingRooms(dungeon, config);
            generation.AddCash(new BorderingRoomsGenerationCash(borderingRooms));
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        private HashSet<int> GetBorderingRooms(Dungeon dungeon, SelectBorderingRoomsGenerationConfig config)
        {
            var minCorridorSize = config.MinCorridorSize;
            var roomsCount = dungeon.Data.RoomsData.Rooms.Count;
            var rooms = dungeon.Data.RoomsData.Rooms;

            var borderingRooms = new HashSet<int>(rooms.Count);
	        
            for (int i = 0; i < roomsCount; ++i)
            {
                var room = rooms[i];
		        
                for (int j = i + 1; j < roomsCount; ++j)
                {
                    var other = rooms[j];
			        
                    var distance = room.GetCenter() - other.GetCenter();
                    var roomDistX = Math.Abs(distance.x);
                    var roomDistY = Math.Abs(distance.y);
			        
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