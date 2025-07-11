using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors
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