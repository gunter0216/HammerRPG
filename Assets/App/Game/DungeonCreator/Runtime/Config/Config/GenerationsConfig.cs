using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;

namespace App.Game.GameManagers.External.Config.Model
{
    public class GenerationsConfig
    {
        private readonly DungeonGenerationConfig[] m_Configs;
        
        public DungeonGenerationConfig[] Configs => m_Configs;

        public GenerationsConfig(DungeonGenerationConfig[] configs)
        {
            m_Configs = configs;
        }
    }
}