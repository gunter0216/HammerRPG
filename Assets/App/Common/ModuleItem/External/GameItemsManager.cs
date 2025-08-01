using System.Collections.Generic;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.DataContainer.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.ModuleItem.External.Config;
using App.Common.ModuleItem.External.Config.Interfaces;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.ModuleItem.Runtime.Fabric;
using App.Common.ModuleItem.Runtime.Fabric.Interfaces;
using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;
using App.Game.Contexts;
using UnityEngine;

namespace App.Common.ModuleItem.External
{
    [Singleton]
    [Stage(typeof(StartSceneContext), 100)]
    public class GameItemsManager : IInitSystem, IGameItemsManager
    {
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly IContainersDataManager m_ContainersDataManager;
        private readonly List<IModuleDtoToConfigConverter> m_ModuleDtoToConfigConverters = new List<IModuleDtoToConfigConverter>();
        private readonly List<ICreateModuleItemHandler> m_Handlers = new List<ICreateModuleItemHandler>();

        private ModuleItemsConfigController m_ConfigController;
        private ModuleItemCreator m_ModuleItemCreator;

        public void Init()
        {
            InitConfigController();
            InitItemsFabric();
        }

        private void InitItemsFabric()
        {
            m_ModuleItemCreator = new ModuleItemCreator(
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

            var dtoConverter = new ModuleItemsDtoToConfigConverter(m_ModuleDtoToConfigConverters);
            var configs = dtoConverter.Convert(dto.Value);
            if (!configs.HasValue)
            {
                Debug.LogError($"[NewGameItemsController] In method Init, cant convert dto to configs.");
                return;
            }
            
            m_ConfigController = new ModuleItemsConfigController(configs.Value);
            m_ConfigController.Initialize();
        }
        
        public Optional<IModuleItem> Create(string id)
        {
            return m_ModuleItemCreator.Create(id);
        }

        public Optional<IModuleItem> Create(IModuleItemData data)
        {
            return m_ModuleItemCreator.Create(data);
        }

        public bool Destroy(IModuleItemData data)
        {
            // todo
            return true;
        }
    }
}