using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.KeysDistributor
{
    internal class VisitedRoomNode
    {
        private readonly DungeonGenerationRoom m_GenerationRoom;
        private bool m_Visited;

        public VisitedRoomNode(DungeonGenerationRoom generationRoom)
        {
            m_GenerationRoom = generationRoom;
        }

        public bool Visited
        {
            get => m_Visited;
            set => m_Visited = value;
        }
    }
}