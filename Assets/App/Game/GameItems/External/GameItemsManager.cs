using System.Collections.Generic;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Configs.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Contexts;
using App.Game.GameItems.Runtime;
using App.Game.GameItems.Runtime.Config;
using App.Game.GameItems.Runtime.Config.Loader;
using App.Game.SpriteLoaders.Runtime;
using App.Game.States.Runtime.Game;

namespace App.Game.GameItems.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 100)]
    public class GameItemsManager : IInitSystem, IGameItemsManager
    {
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly ISpriteLoader m_SpriteLoader;
        [Inject] private readonly ModuleItemsManager m_ModuleItemsManager;
        [Inject] private readonly ILogger m_Logger;

        private GameItemsConfigService m_ConfigService;
        
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