using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;

namespace App.Game.GameManagers.External.Room
{
    public class GameRoom
    {
        private readonly IRoomView m_View;
        private readonly DungeonGenerationRoom m_GenerationRoom;

        public IRoomView View => m_View;

        public DungeonGenerationRoom GenerationRoom => m_GenerationRoom;

        public GameRoom(IRoomView view, DungeonGenerationRoom generationRoom)
        {
            m_View = view;
            m_GenerationRoom = generationRoom;
        }

        public void SetParent(Transform parent)
        {
            m_View.SetParent(parent);
        }
    }
}