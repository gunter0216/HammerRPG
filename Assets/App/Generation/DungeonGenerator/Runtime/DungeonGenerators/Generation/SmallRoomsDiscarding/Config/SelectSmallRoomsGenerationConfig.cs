using System;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Config
{
    [Serializable]
    public class SelectSmallRoomsGenerationConfig : IGenerationConfig
    {
        private readonly int m_HeightRoomThreshold;
        private readonly int m_WidthRoomThreshold;

        public SelectSmallRoomsGenerationConfig(int heightRoomThreshold, int widthRoomThreshold)
        {
            m_HeightRoomThreshold = heightRoomThreshold;
            m_WidthRoomThreshold = widthRoomThreshold;
        }

        public int HeightRoomThreshold => m_HeightRoomThreshold;

        public int WidthRoomThreshold => m_WidthRoomThreshold;
    }
}