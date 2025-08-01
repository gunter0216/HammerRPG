using App.Common.ModuleItem.External.Config.Interfaces;
using App.Common.ModuleItem.External.Dto;
using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;

namespace App.Common.ModuleItem.External.Config
{
    public class GameItemsConfigLoader : IGameItemsConfigLoader
    {
        private readonly IConfigLoader m_ConfigLoader;

        public GameItemsConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<GameItemsDto> Load()
        {
            var dto = m_ConfigLoader.LoadConfig<GameItemsDto>(
                GameItemsConstants.GameItemsConfigLocalKey);

            return dto;
        }
    }
}