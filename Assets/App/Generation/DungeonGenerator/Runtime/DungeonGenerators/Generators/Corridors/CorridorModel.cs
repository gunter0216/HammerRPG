using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors
{
    public struct CorridorModel
    {
        private readonly DungeonGenerationRoom m_GenerationRoom;
        private readonly bool m_IsHorizontal;

        public DungeonGenerationRoom GenerationRoom => m_GenerationRoom;

        public bool IsHorizontal => m_IsHorizontal;

        public CorridorModel(DungeonGenerationRoom generationRoom, bool isHorizontal)
        {
            m_GenerationRoom = generationRoom;
            m_IsHorizontal = isHorizontal;
        }
    }
}