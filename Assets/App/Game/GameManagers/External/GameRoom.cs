using App.Game.GameManagers.External.View;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Game.GameManagers.External
{
    public class GameRoom
    {
        private readonly RoomView m_View;
        private readonly DungeonGenerationRoom m_GenerationRoom;

        public RoomView View => m_View;

        public DungeonGenerationRoom GenerationRoom => m_GenerationRoom;

        public GameRoom(RoomView view, DungeonGenerationRoom generationRoom)
        {
            m_View = view;
            m_GenerationRoom = generationRoom;
        }
    }
}