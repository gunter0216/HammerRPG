using System;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Config
{
    [Serializable]
    public class SelectBorderingRoomsGenerationConfig : IGenerationConfig
    {
        private readonly int m_MinCorridorSize;

        public SelectBorderingRoomsGenerationConfig(int minCorridorSize)
        {
            m_MinCorridorSize = minCorridorSize;
        }

        public int MinCorridorSize => m_MinCorridorSize;
    }
}