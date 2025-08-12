using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Config.Dto;
using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace Assets.App.Game.GameItems.Runtime.Config.Loader
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