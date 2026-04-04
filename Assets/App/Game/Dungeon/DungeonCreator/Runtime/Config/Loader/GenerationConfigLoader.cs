using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Config.Dto;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Config.Loader
{
    public class GenerationConfigLoader
    {
        private const string m_LocalConfigKey = "GenerationConfig";
        
        private readonly IConfigLoader m_ConfigLoader;

        public GenerationConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<GenerationConfigDto> Load()
        {
            return m_ConfigLoader.LoadConfig<GenerationConfigDto>(m_LocalConfigKey);
        }
    }
}