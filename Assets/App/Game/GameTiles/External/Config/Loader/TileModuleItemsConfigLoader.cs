using App.Common.ModuleItem.External.Config.Interfaces;
using App.Common.ModuleItem.External.Dto;
using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;

namespace App.Game.GameTiles.External.Config.Loader
{
    public class TileModuleItemsConfigLoader : IGameItemsConfigLoader
    {
        private const string m_LocalConfigKey = "TileItemsConfig";
        
        private readonly IConfigLoader m_ConfigLoader;

        public TileModuleItemsConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<GameItemsDto> Load()
        {
            return m_ConfigLoader.LoadConfig<GameItemsDto>(m_LocalConfigKey);
        }
    }
}