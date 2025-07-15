using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.KeysDistributor
{
    internal class VisitedRoomNode
    {
        private readonly DungeonRoomData m_Room;
        private bool m_Visited;

        public VisitedRoomNode(DungeonRoomData room)
        {
            m_Room = room;
        }

        public bool Visited
        {
            get => m_Visited;
            set => m_Visited = value;
        }
    }
}