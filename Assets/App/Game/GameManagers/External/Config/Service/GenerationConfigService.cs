using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;
using App.Game.GameManagers.External.Config.Model;
using App.Generation.DungeonGenerator.External.Dto;

namespace App.Game.GameManagers.External.Config.Service
{
    public class GenerationConfigService
    {
        private readonly GenerationConfig m_Config;

        public GenerationConfigService(GenerationConfig config)
        {
            m_Config = config;
        }

        public DungeonGenerationConfigDto GetGeneration()
        {
            return m_Config.Generation;
        }
    }
}