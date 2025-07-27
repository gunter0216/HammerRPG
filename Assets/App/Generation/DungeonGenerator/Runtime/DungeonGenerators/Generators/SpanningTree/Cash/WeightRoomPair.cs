using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash
{
    public readonly struct WeightRoomPair
    {
        private readonly DungeonGenerationRoom m_Room1;
        private readonly DungeonGenerationRoom m_Room2;
        private readonly double m_Weight;

        public WeightRoomPair(DungeonGenerationRoom room1, DungeonGenerationRoom room2, double weight)
        {
            m_Room1 = room1;
            m_Room2 = room2;
            m_Weight = weight;
        }

        public DungeonGenerationRoom Room1 => m_Room1;

        public DungeonGenerationRoom Room2 => m_Room2;

        public double Weight => m_Weight;
    }
}