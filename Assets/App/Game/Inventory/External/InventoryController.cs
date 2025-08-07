using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Windows.External;
using App.Game;
using App.Game.Canvases.External;
using App.Game.Configs.Runtime;
using App.Game.Contexts;
using App.Game.Inventory.External.Config;
using App.Game.Inventory.External.Data;
using App.Game.Inventory.External.View;
using App.Game.Inventory.External.ViewModel;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;
using App.Game.SpriteLoaders.Runtime;
using App.Game.States.Game;
using App.Game.States.Menu;
using Unity.VisualScripting;

namespace App.Menu.Inventory.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class InventoryController : IInitSystem, IInventoryController
    {
        [Inject] private readonly IDataManager m_DataManager;
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly IWindowManager m_WindowManager;
        [Inject] private readonly IAssetManager m_AssetManager;
        [Inject] private readonly PopupCanvas m_PopupCanvas;
        [Inject] private readonly ISpriteLoader m_SpriteLoader;
        [Inject] private readonly IModuleItemsManager m_ModuleItemsManager;
        
        private InventoryDataController m_InventoryDataController;
        private InventoryConfigController m_InventoryConfigController;
        private InventoryWindowModel m_InventoryWindowModel;
        
        public void Init()
        {
            InitData();
            InitConfig();
            InitWindow();
        }

        private void InitWindow()
        {
            m_InventoryWindowModel = new InventoryWindowModel(
                m_WindowManager,
                m_AssetManager,
                m_InventoryDataController,
                m_InventoryConfigController,
                m_PopupCanvas,
                m_SpriteLoader);
        }

        private bool InitConfig()
        {
            var configLoader = new InventoryConfigLoader(m_ConfigLoader);
            var dto = configLoader.Load();
            if (!dto.HasValue)
            {
                HLogger.LogError("InventoryConfig is null");
                return false;
            }
            
            var converter = new InventoryDtoToConfigConverter();
            var config = converter.Convert(dto.Value);
            if (!config.HasValue)
            {
                HLogger.LogError("InventoryConfig conversion failed");
                return false;
            }

            m_InventoryConfigController = new InventoryConfigController(config.Value);
            return true;
        }

        private bool InitData()
        {
            var dataLoader = new InventoryDataLoader(m_DataManager);
            var data = dataLoader.Load();
            if (!data.HasValue)
            {
                HLogger.LogError("InventoryData is null");
                return false;
            }

            m_InventoryDataController = new InventoryDataController(data.Value);

            return true;
        }

        public void OpenWindow()
        {
            m_InventoryWindowModel.Open();
        }

        public void CloseWindow()
        {
            m_InventoryWindowModel.Close();
        }

        public bool IsOpen()
        {
            return m_InventoryWindowModel.IsOpen();
        }

        public bool AddItem(IModuleItemConfig moduleItemConfig)
        {
            if (moduleItemConfig == null)
            {
                HLogger.LogError("Cannot add null item to inventory");
                return false;
            }

            return AddItem(moduleItemConfig.Id);

            
        }

        public bool AddItem(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                HLogger.LogError("Cannot add item with null or empty id to inventory");
                return false;
            }
            
            var item = m_ModuleItemsManager.Create(id);
            if (!item.IsSuccess)
            {
                HLogger.LogError($"Failed to create item with id {id}");
                return false;
            }
            
            HLogger.LogError("Adding item to inventory: " + item.ModuleItem.Id);

            // if (!m_InventoryDataController.AddItem(moduleItemConfig))
            // {
            //     HLogger.LogError($"Failed to add item {moduleItemConfig.Id} to inventory");
            //     return false;
            // }
            //
            // m_InventoryWindowModel.Refresh();
            return true;
        }

        public IReadOnlyList<IInventoryGroupConfig> GetGroups()
        {
            return m_InventoryConfigController.GetGroups();
        }
    }
}