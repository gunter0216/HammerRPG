using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Configs.Runtime;
using App.Common.Data.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.SpriteLoaders.External;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Game.Canvases.External;
using App.Game.Inventory.External.AddItemStrategy;
using App.Game.Inventory.External.ViewModel;
using App.Game.Inventory.Runtime;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Config.Model;
using App.Game.Inventory.Runtime.Data;
using App.Game.Inventory.Runtime.Item;

namespace App.Game.Inventory.External
{
    public class InventoryController : IInitSystem, IInventoryController, IDisposable
    {
        private readonly IDataManager _dataManager;
        private readonly IConfigLoader _configLoader;
        private readonly IWindowManager _windowManager;
        private readonly IAssetManager _assetManager;
        private readonly PopupCanvas _popupCanvas;
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly IModuleItemsManager _moduleItemsManager;

        private InventoryDataService _dataService;
        private InventoryConfigService _configService;
        private InventoryWindowController _inventoryWindowController;
        private InventoryService _service;
        private InventoryAddItemStrategy _addItemStrategy;

        public InventoryController(
            IDataManager dataManager, 
            IConfigLoader configLoader,
            IWindowManager windowManager,
            IAssetManager assetManager,
            PopupCanvas popupCanvas,
            IItemSpriteLoader spriteLoader,
            IModuleItemsManager moduleItemsManager)
        {
            _dataManager = dataManager;
            _configLoader = configLoader;
            _windowManager = windowManager;
            _assetManager = assetManager;
            _popupCanvas = popupCanvas;
            _spriteLoader = spriteLoader;
            _moduleItemsManager = moduleItemsManager;
        }

        public void Init()
        {
            InitConfig();
            InitData();
            InitItems();
            InitWindow();
            _addItemStrategy = new InventoryAddItemStrategy(
                _moduleItemsManager,
                _service,
                _inventoryWindowController);
        }

        private void InitWindow()
        {
            _inventoryWindowController = new InventoryWindowController(
                _windowManager,
                _assetManager,
                _popupCanvas,
                _spriteLoader,
                _service);
            _inventoryWindowController.CreateWindow();
        }

        private bool InitConfig()
        {
            _configService = new InventoryConfigService(_configLoader);
            return _configService.Initialize();
        }

        private bool InitData()
        {
            _dataService = new InventoryDataService(_dataManager);
            return _dataService.Initialize(_configService.GetCols(), _configService.GetRows());
        }

        private bool InitItems()
        {
            _service = new InventoryService(
                _configService,
                _dataService,
                _moduleItemsManager);
            if (!_service.Initialize())
            {
                HLogger.LogError("Failed to initialize InventoryItemsController");
                return false;
            }

            return true;
        }

        public void OpenWindow()
        {
            _inventoryWindowController.Open();
        }

        public void CloseWindow()
        {
            _inventoryWindowController.Close();
        }

        public bool IsOpen()
        {
            return _inventoryWindowController.IsOpen();
        }

        public bool AddItem(IModuleItemConfig moduleItemConfig)
        {
            return _addItemStrategy.AddItem(moduleItemConfig);
        }

        public bool AddItem(string id)
        {
            return _addItemStrategy.AddItem(id);
        }

        public bool AddItem(IModuleItem moduleItem)
        {
            return _addItemStrategy.AddItem(moduleItem);
        }

        public void Remove(InventoryItem inventoryItem)
        {
            _service.Remove(inventoryItem);
        }

        public bool TryGetItem(DataReference dataReference, out InventoryItem item)
        {
            return _service.TryGetItem(dataReference, out item);
        }

        public void Dispose()
        {
            _inventoryWindowController?.Dispose();
        }
    }
}