using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    public class Dungeon
    {
        private readonly DungeonConfig m_Config;
        private readonly DungeonData m_Data;
        
        public DungeonConfig Config => m_Config;
        public DungeonData Data => m_Data;

        public Dungeon(DungeonConfig config, DungeonData data)
        {
            m_Config = config;
            m_Data = data;
        }
        
        public void DiscardRooms(HashSet<int> discardRooms)
        {
            var dungeonRooms = m_Data.RoomsData.Rooms;
            var newRooms = new List<DungeonRoomData>(dungeonRooms.Count);
            for (int i = 0; i < dungeonRooms.Count; ++i)
            {
                var room = dungeonRooms[i];
                if (!discardRooms.Contains(room.UID))
                {
                    newRooms.Add(room);
                }
            }

            m_Data.RoomsData.Rooms = newRooms;
        }
    }
}