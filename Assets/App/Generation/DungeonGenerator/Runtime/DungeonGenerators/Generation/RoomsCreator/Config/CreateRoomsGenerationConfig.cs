using System;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator.Config
{
    [Serializable]
    public class CreateRoomsGenerationConfig : IGenerationConfig
    {
        private readonly int m_CountRooms;

        private readonly int m_MinWidthRoom;
        private readonly int m_MinHeightRoom;

        private readonly int m_MaxWidthRoom;
        private readonly int m_MaxHeightRoom;

        private readonly int m_Radius;

        public CreateRoomsGenerationConfig(
            int countRooms, 
            int minWidthRoom, 
            int minHeightRoom, 
            int maxWidthRoom, 
            int maxHeightRoom, 
            int radius)
        {
            m_CountRooms = countRooms;
            m_MinWidthRoom = minWidthRoom;
            m_MinHeightRoom = minHeightRoom;
            m_MaxWidthRoom = maxWidthRoom;
            m_MaxHeightRoom = maxHeightRoom;
            m_Radius = radius;
        }

        public int CountRooms => m_CountRooms;

        public int MinWidthRoom => m_MinWidthRoom;

        public int MinHeightRoom => m_MinHeightRoom;

        public int MaxWidthRoom => m_MaxWidthRoom;

        public int MaxHeightRoom => m_MaxHeightRoom;

        public int Radius => m_Radius;
    }
}