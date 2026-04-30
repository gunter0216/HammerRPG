using System.Collections.Generic;
using App.Game.Dungeon.DungeonCreator.Runtime.Rooms;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;

namespace App.Game.Dungeon.DungeonCreator.Runtime
{
    public class Dungeon
    {
        private readonly DungeonData m_Data;
        private readonly DungeonGenerationConfig m_Config;
        
        private List<Room> m_Rooms;
        private Room m_StartRoom;
        private Room m_EndRoom;

        public Dungeon(DungeonData data, DungeonGenerationConfig config)
        {
            m_Data = data;
            m_Config = config;
        }

        public DungeonData Data => m_Data;

        public DungeonGenerationConfig Config => m_Config;

        public List<Room> Rooms
        {
            get => m_Rooms;
            set => m_Rooms = value;
        }

        public Room EndRoom
        {
            get => m_EndRoom;
            set => m_EndRoom = value;
        }

        public Room StartRoom
        {
            get => m_StartRoom;
            set => m_StartRoom = value;
        }
    }
}