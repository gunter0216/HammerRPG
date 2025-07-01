namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators
{
    public class Dungeon
    {
        private readonly DungeonConfig m_Config;
        private readonly DungeonData m_Data;
        
        public DungeonConfig Config => m_Config;
        public DungeonData Data => m_Data;

        public Dungeon(DungeonConfig config, DungeonData data)
        {
            m_Config = config;
            m_Data = data;
        }
    }
}