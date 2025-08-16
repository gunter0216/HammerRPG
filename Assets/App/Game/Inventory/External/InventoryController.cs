using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Configs.Runtime;
using App.Common.Data.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Windows.External;
using App.Game.Canvases.External;
using App.Game.Contexts;
using App.Game.Inventory.External.AddItemStrategy;
using App.Game.Inventory.External.Group;
using App.Game.Inventory.External.ViewModel;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;
using App.Game.SpriteLoaders.Runtime;
using App.Game.States.Runtime.Game;

namespace App.Game.Inventory.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 1000)]
    public class InventoryController : IInitSystem, IInventoryController, IDisposable
    {
        [Inject] private readonly IDataManager m_DataManager;
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly IWindowManager m_WindowManager;
        [Inject] private readonly IAssetManager m_AssetManager;
        [Inject] private readonly PopupCanvas m_PopupCanvas;
        [Inject] private readonly ISpriteLoader m_SpriteLoader;
        [Inject] private readonly IModuleItemsManager m_ModuleItemsManager;
        
        private InventoryDataController m_DataController;
        private InventoryConfigController m_ConfigController;
        private InventoryWindowModel m_InventoryWindowModel;
        private InventoryItemsController m_ItemsController;
        private InventoryGroupController m_GroupController;
        private InventoryAddItemStrategy m_AddItemStrategy;
        
        public void Init()
        {
            InitData();
            InitConfig();
            InitGroup();
            InitItems();
            InitWindow();
            m_AddItemStrategy = new InventoryAddItemStrategy(
                m_ModuleItemsManager,
                m_ItemsController,
                m_InventoryWindowModel);
        }

        private void InitWindow()
        {
            m_InventoryWindowModel = new InventoryWindowModel(
                m_WindowManager,
                m_AssetManager,
                m_DataController,
                m_ConfigController,
                m_PopupCanvas,
                m_SpriteLoader,
                m_GroupController,
                m_ItemsController);
        }

        private bool InitConfig()
        {
            m_ConfigController = new InventoryConfigController(m_ConfigLoader);
            return m_ConfigController.Initialize();
        }

        private bool InitData()
        {
            m_DataController = new InventoryDataController(m_DataManager);
            return m_DataController.Initialize();
        }

        private bool InitGroup()
        {
            m_GroupController = new InventoryGroupController(m_ConfigController);
            if (!m_GroupController.Initialize())
            {
                HLogger.LogError("Failed to initialize InventoryGroupController");
                return false; 
            }

            return true;
        }

        private bool InitItems()
        {
            m_ItemsController = new InventoryItemsController(
                m_ConfigController,
                m_DataController,
                m_ModuleItemsManager,
                m_GroupController);
            if (!m_ItemsController.Initialize())
            {
                HLogger.LogError("Failed to initialize InventoryItemsController");
                return false; 
            }

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
            return m_AddItemStrategy.AddItem(moduleItemConfig);
        }

        public bool AddItem(string id)
        {
            return m_AddItemStrategy.AddItem(id);
        }

        public bool AddItem(IModuleItem moduleItem)
        {
            return m_AddItemStrategy.AddItem(moduleItem);
        }

        public IReadOnlyList<IInventoryGroupConfig> GetGroups()
        {
            return m_ConfigController.GetGroups();
        }

        public void Dispose()
        {
            m_InventoryWindowModel?.Dispose();
        }
    }
}