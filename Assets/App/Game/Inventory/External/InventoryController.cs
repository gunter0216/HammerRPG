using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Configs.Runtime;
using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Game.Canvases.External;
using App.Game.Inventory.External.AddItemStrategy;
using App.Game.Inventory.External.Group;
using App.Game.Inventory.External.ViewModel;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;

namespace App.Game.Inventory.External
{
    public class InventoryController : IInitSystem, IInventoryController, IDisposable
    {
        private readonly IDataManager m_DataManager;
        private readonly IConfigLoader m_ConfigLoader;
        private readonly IWindowManager m_WindowManager;
        private readonly IAssetManager m_AssetManager;
        private readonly PopupCanvas m_PopupCanvas;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly IModuleItemsManager m_ModuleItemsManager;

        private InventoryDataController m_DataController;
        private InventoryConfigController m_ConfigController;
        private InventoryWindowModel m_InventoryWindowModel;
        private InventoryItemsController m_ItemsController;
        private InventoryGroupController m_GroupController;
        private InventoryAddItemStrategy m_AddItemStrategy;

        public InventoryController(
            IDataManager dataManager, 
            IConfigLoader configLoader,
            IWindowManager windowManager,
            IAssetManager assetManager,
            PopupCanvas popupCanvas,
            ISpriteLoader spriteLoader,
            IModuleItemsManager moduleItemsManager)
        {
            m_DataManager = dataManager;
            m_ConfigLoader = configLoader;
            m_WindowManager = windowManager;
            m_AssetManager = assetManager;
            m_PopupCanvas = popupCanvas;
            m_SpriteLoader = spriteLoader;
            m_ModuleItemsManager = moduleItemsManager;
        }

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