using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors
{
    public class RoomConnection
    {
        private readonly DungeonGenerationRoom m_GenerationRoom;
        private readonly RoomConnectSide m_Side;

        public RoomConnection(DungeonGenerationRoom generationRoom, RoomConnectSide side)
        {
            m_GenerationRoom = generationRoom;
            m_Side = side;
        }

        public DungeonGenerationRoom GenerationRoom => m_GenerationRoom;

        public RoomConnectSide Side => m_Side;
    }
}