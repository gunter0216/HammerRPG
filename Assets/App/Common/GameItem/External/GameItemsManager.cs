using System.Collections.Generic;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.DataContainer.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.GameItem.External.Config;
using App.Common.GameItem.External.Config.Interfaces;
using App.Common.GameItem.Runtime;
using App.Common.GameItem.Runtime.Config;
using App.Common.GameItem.Runtime.Data;
using App.Common.GameItem.Runtime.Fabric;
using App.Common.GameItem.Runtime.Fabric.Interfaces;
using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;
using App.Game.Contexts;
using UnityEngine;

namespace App.Common.GameItem.External
{
    [Singleton]
    [Stage(typeof(StartSceneContext), 100)]
    public class GameItemsManager : IInitSystem, IGameItemsManager
    {
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly IContainersDataManager m_ContainersDataManager;
        private readonly List<IModuleDtoToConfigConverter> m_ModuleDtoToConfigConverters = new List<IModuleDtoToConfigConverter>();
        private readonly List<ICreateGameItemHandler> m_Handlers = new List<ICreateGameItemHandler>();

        private GameItemConfigController m_ConfigController;
        private GameItemCreator m_GameItemCreator;

        public void Init()
        {
            InitConfigController();
            InitItemsFabric();
        }

        private void InitItemsFabric()
        {
            m_GameItemCreator = new GameItemCreator(
                m_ConfigController, 
                m_ContainersDataManager, 
                m_Handlers);
        }

        private void InitConfigController()
        {
            var loader = new GameItemsConfigLoader(m_ConfigLoader);
            var dto = loader.Load();
            if (!dto.HasValue)
            {
                Debug.LogError($"[NewGameItemsController] In method Init, cant load file.");
                return;
            }

            var dtoConverter = new GameItemsDtoToConfigConverter(m_ModuleDtoToConfigConverters);
            var configs = dtoConverter.Convert(dto.Value);
            if (!configs.HasValue)
            {
                Debug.LogError($"[NewGameItemsController] In method Init, cant convert dto to configs.");
                return;
            }
            
            m_ConfigController = new GameItemConfigController(configs.Value);
            m_ConfigController.Initialize();
        }
        
        public Optional<IGameItem> Create(string id)
        {
            return m_GameItemCreator.Create(id);
        }

        public Optional<IGameItem> Create(IGameItemData data)
        {
            return m_GameItemCreator.Create(data);
        }

        public bool Destroy(IGameItemData data)
        {
            // todo
            return true;
        }
    }
}