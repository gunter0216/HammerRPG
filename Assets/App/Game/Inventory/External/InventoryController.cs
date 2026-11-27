using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
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
using App.Game.DragItem.Runtime;
using App.Game.Inventory.External.AddItemStrategy;
using App.Game.Inventory.External.ViewModel;
using App.Game.Inventory.Runtime;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Config.Model;
using App.Game.Inventory.Runtime.Data;
using App.Game.Inventory.Runtime.Item;
using App.Game.Modules.Equipment.Runtime;

namespace App.Game.Inventory.External
{
    public class InventoryController : IInitSystem, IInventoryController, IDisposable
    {
        private readonly IConfigLoader _configLoader;
        private readonly IWindowManager _windowManager;
        private readonly IAssetManager _assetManager;
        private readonly ICanvasController _canvasController;
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly DragItemController _dragItemController;

        private InventoryConfigService _configService;
        private InventoryWindowController _inventoryWindowController;
        private InventoryService _service;
        private InventoryAddItemStrategy _addItemStrategy;

        internal event Action WindowClosed;
        internal EquipmentModule Equipment { get; private set; }

        public InventoryController(
            IConfigLoader configLoader,
            IWindowManager windowManager,
            IAssetManager assetManager,
            IItemSpriteLoader spriteLoader,
            IModuleItemsManager moduleItemsManager,
            ICanvasController canvasController,
            DragItemController dragItemController)
        {
            _configLoader = configLoader;
            _windowManager = windowManager;
            _assetManager = assetManager;
            _spriteLoader = spriteLoader;
            _moduleItemsManager = moduleItemsManager;
            _canvasController = canvasController;
            _dragItemController = dragItemController;
        }

        public void Init()
        {
            if (!InitConfig() || !InitItems() || !InitWindow())
            {
                HLogger.LogError("Could not initialize inventory window.");
                return;
            }
            _addItemStrategy = new InventoryAddItemStrategy(
                _moduleItemsManager,
                _service,
                _inventoryWindowController);
            _service.ItemChanged += _inventoryWindowController.AddItem;
        }

        public bool BindContainer(App.Game.Containers.Container.Runtime.Container container)
        {
            return _addItemStrategy != null && _service.BindContainer(container);
        }

        public bool BindEquipment(EquipmentModule equipment)
        {
            if (equipment == null || _addItemStrategy == null || !_inventoryWindowController.BindEquipment(equipment))
                return false;

            Equipment = equipment;
            return true;
        }

        private bool InitWindow()
        {
            _inventoryWindowController = new InventoryWindowController(
                _windowManager,
                _assetManager,
                _canvasController,
                _spriteLoader,
                _service,
                _dragItemController);
            _inventoryWindowController.Closed += () => WindowClosed?.Invoke();
            return _inventoryWindowController.CreateWindow();
        }

        private bool InitConfig()
        {
            _configService = new InventoryConfigService(_configLoader);
            return _configService.Initialize();
        }

        private bool InitItems()
        {
            _service = new InventoryService(_configService);
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
            if (_service != null)
            {
                if (_inventoryWindowController != null)
                    _service.ItemChanged -= _inventoryWindowController.AddItem;
                _service.Dispose();
            }
            _inventoryWindowController?.Dispose();
        }
    }
}
