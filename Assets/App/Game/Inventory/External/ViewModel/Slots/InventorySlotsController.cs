using System;
using System.Collections.Generic;
using App.Common.Events.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.External;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.Runtime.Events;
using App.Game.DragItem.Runtime.Model;
using App.Game.GameTiles.External.Config.Model;
using App.Game.Inventory.External.View;
using App.Game.Inventory.External.ViewModel.Fabric;
using App.Game.Inventory.Runtime.Item;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel.Slots
{
    public class InventorySlotsController : IDisposable
    {
        private readonly InventoryWindow _window;
        private readonly InventorySlotViewCreator _slotViewCreator;
        private readonly InventoryService _service;
        private readonly IItemSpriteLoader _spriteLoader;
        
        private List<ItemSlotController> _slots;
        
        public InventorySlotsController(
            InventorySlotViewCreator slotViewCreator, 
            InventoryService service, 
            IItemSpriteLoader spriteLoader, 
            InventoryWindow window)
        {
            _slotViewCreator = slotViewCreator;
            _service = service;
            _spriteLoader = spriteLoader;
            _window = window;
        }

        public void Initialize()
        {
            CreateSlots();
        }

        public void OnWindowOpened()
        {
            UpdateSlots();
        }

        private void CreateSlots()
        {
            var slots = _service.GetItems();
            _slots = new List<ItemSlotController>(slots.Count);
            for (int i = 0; i < slots.Count; ++i)
            {
                var view = _slotViewCreator.Create();
                if (!view.HasValue)
                {
                    HLogger.LogError($"Failed to create slot view");
                    return;
                }
                    
                var viewModel = new ItemSlotController(
                    view.Value,
                    i,
                    clickCallback: OnSlotClick,
                    canPlaceFunc: CanPlace,
                    placeFunc: Place,
                    removeFunc: Remove);
                viewModel.Initialize();
                _slots.Add(viewModel);
            }
        }

        private Optional<IModuleItem> Remove(ItemSlotController slot)
        {
            var item = slot.GetItem();
            if (item == null)
            {
                HLogger.LogError("Slot is empty");
                return Optional<IModuleItem>.Fail();
            }

            slot.Clear();
            _service.RemoveItem(slot.Index);
            
            return Optional<IModuleItem>.Success(item);
        }

        private bool Place(ItemSlotController slot, IModuleItem item)
        {
            if (slot.HasItem())
            {
                HLogger.LogError($"slot is not empty {slot.GetItem().Id}");
                return false;
            }

            UpdateSlot(slot, item);
            _service.AddItem(item, slot.Index);
            return true;
        }

        private bool CanPlace(ItemSlotController controller, IModuleItem item)
        {
            return true;
        }

        private void OnSlotClick(ItemSlotController controller)
        {
            EventManager.Trigger(new ItemSlotClickEvent(controller));
        }

        private void UpdateSlots()
        {
            HideAllItems();
            var items = _service.GetItems();
            foreach (var item in items)
            {
                UpdateSlot(item);
            }
        }

        public void UpdateSlot(InventoryItem item)
        {
            var slot = _slots[item.Data.Index];

            UpdateSlot(slot, item.Item);
        }

        private void UpdateSlot(ItemSlotController slot, IModuleItem item)
        {
            Sprite sprite = null;
            if (item != null)
            {
                sprite = GetSprite(item).Value;
            }

            slot.SetItem(item, sprite);
        }

        public void OnWindowClosed()
        {
        }

        private void HideAllItems()
        {
            foreach (var slot in _slots)
            {
                slot.Clear();
            }
        }

        public Optional<Sprite> GetSprite(IModuleItem item)
        {
            var sprite = _spriteLoader.LoadItemSprite(item);
            if (!sprite.HasValue)
            {
                HLogger.LogError($"Failed to load sprite for item");
                return Optional<Sprite>.Fail();
            }

            return sprite;
        }

        public void Dispose()
        {
        }
    }
}