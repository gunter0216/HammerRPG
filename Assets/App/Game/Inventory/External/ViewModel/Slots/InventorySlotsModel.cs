using System;
using System.Collections.Generic;
using App.Common.Events.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.External.Events;
using App.Game.GameTiles.External.Config.Model;
using App.Game.Inventory.External.Services;
using App.Game.Inventory.External.View;
using App.Game.Inventory.Runtime.Config;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventorySlotsModel : IDisposable
    {
        private readonly InventoryWindow m_Window;
        private readonly IInventoryConfigController m_ConfigController;
        private readonly InventorySlotViewCreator m_SlotViewCreator;
        private readonly InventoryItemsController m_ItemsController;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private List<ItemSlotModel> m_Slots;
        
        private IInventoryGroupConfig m_Group;

        public InventorySlotsModel(
            IInventoryConfigController configController, 
            InventorySlotViewCreator slotViewCreator, 
            InventoryItemsController itemsController, 
            ISpriteLoader spriteLoader, 
            InventoryWindow window)
        {
            m_ConfigController = configController;
            m_SlotViewCreator = slotViewCreator;
            m_ItemsController = itemsController;
            m_SpriteLoader = spriteLoader;
            m_Window = window;
        }

        public void Initialize()
        {
            CreateSlots();
            
            m_Window.SetBlockButtonClickCallback(OnBlockButtonClick);
        }

        private void OnBlockButtonClick()
        {
            // todo выкидывать предмет
        }

        private void CreateSlots()
        {
            var rows = m_ConfigController.GetRows();
            var columns = m_ConfigController.GetCols();
            var countSlots = columns * rows;
            m_Slots = new List<ItemSlotModel>(countSlots);
            for (int i = 0; i < countSlots; ++i)
            {
                var view = m_SlotViewCreator.Create();
                if (!view.HasValue)
                {
                    HLogger.LogError($"Failed to create slot view");
                    return;
                }
                    
                var viewModel = new ItemSlotModel(
                    view.Value,
                    i,
                    clickCallback: OnSlotClick,
                    canPlaceFunc: CanPlace,
                    placeFunc: Place,
                    removeFunc: Remove);
                viewModel.Initialize();
                m_Slots.Add(viewModel);
            }
        }

        private Optional<IModuleItem> Remove(ItemSlotModel slot)
        {
            var item = slot.GetItem();
            if (item == null)
            {
                HLogger.LogError("Slot is empty");
                return Optional<IModuleItem>.Fail();
            }

            slot.Clear();
            m_ItemsController.RemoveItem(item, slot.Index);
            
            return Optional<IModuleItem>.Success(item);
        }

        private bool Place(ItemSlotModel slot, IModuleItem item)
        {
            if (slot.HasItem())
            {
                HLogger.LogError($"slot is not empty {slot.GetItem().Id}");
                return false;
            }

            UpdateSlot(slot, item);
            m_ItemsController.AddItem(item, slot.Index);
            return true;
        }

        private bool CanPlace(ItemSlotModel model, IModuleItem item)
        {
            return !model.HasItem();
        }
        
        private void OnSlotClick(ItemSlotModel model)
        {
            EventManager.Trigger(new ItemSlotClickEvent(model));
        }

        public void ShowGroup(IInventoryGroupConfig group)
        {
            m_Group = group;
            HideAllItems();
            var items = m_ItemsController.GetItemsByGroup(group);
            if (!items.HasValue)
            {
                HLogger.LogError("not found items in group");
                return;
            }

            for (int i = 0; i < items.Value.Count; ++i)
            {
                var item = items.Value[i];
                if (item == null)
                {
                    continue;
                }
                
                UpdateSlot(item);
            }
        }

        public void UpdateSlot(InventoryItem item)
        {
            var slot = m_Slots[item.Data.Index];

            UpdateSlot(slot, item.Item);
        }

        private void UpdateSlot(ItemSlotModel slot, IModuleItem item)
        {
            var sprite = GetSprite(item);
            slot.SetItem(item, sprite.Value);
        }

        public void OnWindowClosed()
        {
        }
        
        private void HideAllItems()
        {
            foreach (var slot in m_Slots)
            {
                slot.Clear();
            }
        }
        
        public Optional<Sprite> GetSprite(IModuleItem item)
        {
            var spriteModule = item.GetConfigModule<SpriteModuleConfig>();
            if (!spriteModule.HasValue)
            {
                HLogger.LogError("SpriteModuleConfig is not available for the item.");
                return Optional<Sprite>.Fail();
            }
            
            var sprite = m_SpriteLoader.Load(spriteModule.Value.Key);
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