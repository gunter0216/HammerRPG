using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameManagers.External.Config.Dto;
using App.Game.GameManagers.External.Config.Model;

namespace App.Game.GameManagers.External.Config.Loader
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