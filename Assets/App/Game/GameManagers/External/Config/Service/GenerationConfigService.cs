using App.Game.GameManagers.External.Config.Model;
using App.Generation.DungeonGenerator.External.Dto;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;

namespace App.Game.GameManagers.External.Config.Service
{
    public class GenerationConfigService
    {
        private readonly GenerationConfig m_Config;

        public GenerationConfigService(GenerationConfig config)
        {
            m_Config = config;
        }

        public DungeonGenerationConfig GetGeneration()
        {
            return m_Config.Generation;
        }
    }
}