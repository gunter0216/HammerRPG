using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors
{
    public struct CorridorModel
    {
        private readonly DungeonRoomData m_Room;
        private readonly bool m_IsHorizontal;

        public DungeonRoomData Room => m_Room;

        public bool IsHorizontal => m_IsHorizontal;

        public CorridorModel(DungeonRoomData room, bool isHorizontal)
        {
            m_Room = room;
            m_IsHorizontal = isHorizontal;
        }
    }
}