using System.Collections.Generic;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Game.DungeonGenerator.Runtime.Rooms;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsDiscarding
{
    public class SmallRoomsDiscarder
    {
        public HashSet<int> GetSmallRooms(Dungeon dungeon)
        {
            var smallRooms = new HashSet<int>(dungeon.Data.RoomsData.Rooms.Count);
            for(int i = 0; i < dungeon.Data.RoomsData.Rooms.Count; ++i)
            {
                var room = dungeon.Data.RoomsData.Rooms[i];
		        
                if (room.Width < dungeon.Config.SmallRooms.WidthRoomThreshold || 
                    room.Height < dungeon.Config.SmallRooms.HeightRoomThreshold)
                {
                    smallRooms.Add(room.UID);
                }
            }

            return smallRooms;
        }

        public void DiscardRooms(Dungeon dungeon, HashSet<int> discardRooms)
        {
            dungeon.DiscardRooms(discardRooms);
        }
    }
}