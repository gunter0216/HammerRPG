using App.Common.ModuleItem.External.Config.Interfaces;
using App.Common.ModuleItem.External.Dto;
using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;

namespace App.Game.GameItems.External.Config.Loader
{
    public class GameModuleItemsConfigLoader : IModuleItemsConfigLoader
    {
        private const string m_LocalConfigKey = "GameModuleItemsConfig";
        
        private readonly IConfigLoader m_ConfigLoader;

        public GameModuleItemsConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<ModuleItemsDto> Load()
        {
            var dto = m_ConfigLoader.LoadConfig<ModuleItemsDto>(
                m_LocalConfigKey);

            return dto;
        }
    }
}