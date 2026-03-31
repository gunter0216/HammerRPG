using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Windows.External;
using App.Game.Canvases.External;
using App.Game.Inventory.External.View;
using App.Game.Inventory.External.ViewModel.Fabric;
using App.Game.Inventory.External.ViewModel.Slots;
using App.Game.Inventory.Runtime.Item;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventoryWindowController : IDisposable
    {
        private readonly IWindowManager _windowManager;
        private readonly IAssetManager _assetManager;
        private readonly ICanvas _canvas;
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly InventoryService _service;

        private InventoryWindow _window;
        
        private InventorySlotsController _slotsController;

        public InventoryWindowController(
            IWindowManager windowManager,
            IAssetManager assetManager,
            ICanvas canvas,
            IItemSpriteLoader spriteLoader,
            InventoryService service)
        {
            _windowManager = windowManager;
            _assetManager = assetManager;
            _canvas = canvas;
            _spriteLoader = spriteLoader;
            _service = service;
        }

        public void Open()
        {
            if (_window == null)
            {
                if (!CreateWindow())
                {
                    HLogger.LogError("Failed to create inventory window.");
                    return;
                }
            }

            _window.SetActive(true);
            _slotsController.OnWindowOpened();
        }

        public void Close()
        {
            _window.SetActive(false);
            _slotsController.OnWindowClosed();
        }
        
        public bool IsOpen()
        {
            return _window != null && _window.IsActive();
        }
        
        private bool CreateWindow()
        {
            var windowCreator = new InventoryWindowCreator(_assetManager, _canvas);
            var window = windowCreator.Create();
            if (!window.HasValue)
            {
                return false;
            }

            _window = window.Value;
            InitWindow();
            
            return true;
        }

        private void InitWindow()
        {
            InitSlots();
            
            _window.SetCloseButtonClickCallback(OnCloseButtonClick);
        }
        
        private void OnCloseButtonClick()
        {
            Close();
        }

        private void InitSlots()
        {
            _slotsController = new InventorySlotsController(new InventorySlotViewCreator(_window),
                _service,
                _spriteLoader,
                _window);
            _slotsController.Initialize();
            _window.ItemsContent.transform.SetAsLastSibling();
        }

        public void AddItem(InventoryItem item)
        {
            if (!IsOpen())
            {
                return;
            }

            _slotsController.UpdateSlot(item);
        }

        public void Dispose()
        {
            _slotsController?.Dispose();
        }
    }
}