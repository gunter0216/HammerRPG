using System.Collections.Generic;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator;
using App.Game.DungeonGenerator.Runtime.Rooms;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors
{
    public class CorridorsCreator
    {
        private readonly RoomCreator m_RoomCreator;

        public CorridorsCreator(RoomCreator roomCreator)
        {
            m_RoomCreator = roomCreator;
        }

        public List<DungeonRoomData> CreateRooms(Dungeon dungeon)
        {
            return null;
        }
    }
}