using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Windows.External;
using App.Common.Windows.Runtime;
using App.Game.Canvases.External;
using App.Game.DragItem.Runtime;
using App.Game.Inventory.External.View;
using App.Game.Inventory.External.ViewModel.Fabric;
using App.Game.Inventory.External.ViewModel.Slots;
using App.Game.Inventory.Runtime.Item;
using App.Game.Modules.Equipment.Runtime;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventoryWindowController : BaseWindowController<InventoryWindow>, IDisposable
    {
        public const string WindowKey = "InventoryWindow";
        
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly InventoryService _service;
        private readonly DragItemController _dragItemController;

        private InventorySlotsController _slotsController;

        public InventoryWindowController(
            IWindowManager windowManager,
            IAssetManager assetManager,
            ICanvasController canvasController,
            IItemSpriteLoader spriteLoader,
            InventoryService service,
            DragItemController dragItemController) : base(windowManager, assetManager, canvasController)
        {
            _spriteLoader = spriteLoader;
            _service = service;
            _dragItemController = dragItemController;
        }

        protected override void OnInitWindow()
        {
            base.OnInitWindow();
            
            InitSlots();
            if (!_window.InitializeItemHover(_dragItemController))
                HLogger.LogError("Could not initialize inventory item info window.");
            
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

        public bool BindEquipment(EquipmentModule equipment)
        {
            var slots = _window.EquipmentSlots;
            if (slots == null || slots.Count == 0)
                return false;

            foreach (var slot in slots)
            {
                if (slot == null)
                    return false;
            }

            foreach (var slot in slots)
                slot.Bind(equipment, _dragItemController, _spriteLoader);
            return true;
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
            if (_window != null && _window.EquipmentSlots != null)
            {
                foreach (var slot in _window.EquipmentSlots)
                {
                    if (slot != null)
                        slot.Unbind();
                }
            }
            _slotsController?.Dispose();
        }
    }
}
