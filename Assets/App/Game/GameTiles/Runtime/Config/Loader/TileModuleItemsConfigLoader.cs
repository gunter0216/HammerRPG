using App.Common.Configs.Runtime;
using App.Common.ModuleItem.Runtime.Config.Dto;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.GameTiles.External.Config.Loader
{
    public class TileModuleItemsConfigLoader : IModuleItemsConfigLoader
    {
        private const string m_LocalConfigKey = "TileItemsConfig";
        
        private readonly IConfigLoader m_ConfigLoader;

        public TileModuleItemsConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<ModuleItemsDto> Load()
        {
            return m_ConfigLoader.LoadConfig<ModuleItemsDto>(m_LocalConfigKey);
        }
    }
}