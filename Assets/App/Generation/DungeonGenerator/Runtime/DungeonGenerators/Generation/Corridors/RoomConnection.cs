using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors
{
    public class RoomConnection
    {
        private readonly DungeonRoomData m_Room;
        private readonly RoomConnectSide m_Side;

        public RoomConnection(DungeonRoomData room, RoomConnectSide side)
        {
            m_Room = room;
            m_Side = side;
        }

        public DungeonRoomData Room => m_Room;

        public RoomConnectSide Side => m_Side;
    }
}