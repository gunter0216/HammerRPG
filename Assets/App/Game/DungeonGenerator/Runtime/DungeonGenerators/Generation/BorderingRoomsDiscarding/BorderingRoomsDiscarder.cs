using System;
using System.Collections.Generic;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding
{
    public class BorderingRoomsDiscarder
    {
        public HashSet<int> GetBorderingRooms(Dungeon dungeon)
        {
            var minCorridorSize = dungeon.Config.BorderingRooms.MinCorridorSize;
            var roomsCount = dungeon.Data.RoomsData.Rooms.Count;

            var borderingRooms = new HashSet<int>(dungeon.Data.RoomsData.Rooms.Count);
	        
            for (int i = 0; i < roomsCount; ++i)
            {
                var room = dungeon.Data.RoomsData.Rooms[i];
		        
                for (int j = i + 1; j < roomsCount; ++j)
                {
                    var other = dungeon.Data.RoomsData.Rooms[j];
			        
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
    }
}