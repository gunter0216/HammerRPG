using System.Collections.Generic;
using App.Game.GameManagers.External.Room;

namespace App.Game.GameManagers.External.Fabric
{
    public readonly struct CreateRoomsResult
    {
        private readonly List<GameRoom> m_Rooms;
        private readonly GameRoom m_EndRoom;
        private readonly GameRoom m_StartRoom;

        public List<GameRoom> Rooms => m_Rooms;

        public GameRoom EndRoom => m_EndRoom;

        public GameRoom StartRoom => m_StartRoom;

        public CreateRoomsResult(List<GameRoom> rooms, GameRoom endRoom, GameRoom startRoom)
        {
            m_Rooms = rooms;
            m_EndRoom = endRoom;
            m_StartRoom = startRoom;
        }
    }
}