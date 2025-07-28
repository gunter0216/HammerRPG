using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;
using App.Game.GameTiles.External.Config.Dto;

namespace App.Game.GameTiles.External.Config.Loader
{
    public class TilesConfigLoader
    {
        private const string m_LocalConfigKey = "TilesConfig";
        
        private readonly IConfigLoader m_ConfigLoader;

        public TilesConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<TilesConfigDto> Load()
        {
            return m_ConfigLoader.LoadConfig<TilesConfigDto>(m_LocalConfigKey);
        }
    }
}