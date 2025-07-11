using System;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsSeparator.Config
{
    [Serializable]
    public class SeparateRoomsGenerationConfig : IGenerationConfig
    {
        private readonly int m_Speed;

        public SeparateRoomsGenerationConfig(int speed)
        {
            m_Speed = speed;
        }

        public int Speed => m_Speed;
    }
}