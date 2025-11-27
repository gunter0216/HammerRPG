using System.Collections.Generic;
using App.Common.Configs.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameItems.Runtime;
using App.Game.GameItems.Runtime.Config;
using App.Game.GameItems.Runtime.Config.Loader;

namespace App.Game.GameItems.External
{
    public class GameItemsManager : IInitSystem, IGameItemsManager
    {
        private readonly IConfigLoader m_ConfigLoader;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly ModuleItemsManager m_ModuleItemsManager;
        private readonly ILogger m_Logger;

        private GameItemsConfigService m_ConfigService;

        public GameItemsManager(
            IConfigLoader configLoader, 
            ISpriteLoader spriteLoader, 
            ModuleItemsManager moduleItemsManager, 
            ILogger logger)
        {
            m_ConfigLoader = configLoader;
            m_SpriteLoader = spriteLoader;
            m_ModuleItemsManager = moduleItemsManager;
            m_Logger = logger;
        }

        public void Init()
        {
            var configLoader = new GameModuleItemsConfigLoader(m_ConfigLoader);
            m_ModuleItemsManager.RegisterItems(configLoader, GameItemsConstants.ModuleItemType);

            var configs = m_ModuleItemsManager.GetConfigs(GameItemsConstants.ModuleItemType);
            if (!configs.HasValue)
            {
                return;
            }

            m_ConfigService = new GameItemsConfigService(m_Logger);
            m_ConfigService.SetItems(configs.Value);
        }

        public Optional<IGameModuleItem> Create(DataReference dataReference)
        {
            var item = m_ModuleItemsManager.Create(dataReference);
            if (!item.HasValue)
            {
                return Optional<IGameModuleItem>.Fail();
            }
            
            return Optional<IGameModuleItem>.Success(new GameModuleItem(item.Value));
        }

        public Optional<IGameModuleItem> Create(string id)
        {
            var item = m_ModuleItemsManager.Create(id);
            if (!item.HasValue)
            {
                return Optional<IGameModuleItem>.Fail();
            }
            
            return Optional<IGameModuleItem>.Success(new GameModuleItem(item.Value));
        }

        public bool Destroy(IGameModuleItem data)
        {
            return m_ModuleItemsManager.Destroy(data);
        }

        public Optional<IReadOnlyList<IModuleItemConfig>> GetItemsByType(string type)
        {
            return m_ConfigService.GetItemsByType(type);
        }
    }
}