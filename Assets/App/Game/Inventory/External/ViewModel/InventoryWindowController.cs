using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Windows.External;
using App.Common.Windows.Runtime;
using App.Game.Canvases.External;
using App.Game.Inventory.External.View;
using App.Game.Inventory.External.ViewModel.Fabric;
using App.Game.Inventory.External.ViewModel.Slots;
using App.Game.Inventory.Runtime.Item;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventoryWindowController : BaseWindowController<InventoryWindow>, IDisposable
    {
        public const string WindowKey = "InventoryWindow";
        
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly InventoryService _service;

        private InventorySlotsController _slotsController;

        public InventoryWindowController(
            IWindowManager windowManager,
            IAssetManager assetManager,
            ICanvas canvas,
            IItemSpriteLoader spriteLoader,
            InventoryService service) : base(windowManager, assetManager, canvas)
        {
            _spriteLoader = spriteLoader;
            _service = service;
        }

        protected override void OnInitWindow()
        {
            base.OnInitWindow();
            
            InitSlots();
            
            _window.SetCloseButtonClickCallback(OnCloseButtonClick);
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            
            _slotsController.OnWindowOpened();
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

        protected override string GetWindowAssetKey()
        {
            return WindowKey;
        }

        public override WindowNames GetName()
        {
            return WindowNames.Inventory;
        }

        public void Dispose()
        {
            _slotsController?.Dispose();
        }
    }
}