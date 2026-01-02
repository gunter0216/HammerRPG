using System.Collections.Generic;
using App.Common.Events.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.Runtime.Events;
using App.Game.DragItem.Runtime.Model;
using App.Game.Equipment.External.View;
using App.Game.GameTiles.External.Config.Model;
using App.Game.Inventory.External;
using App.Game.Inventory.External.ViewModel;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Item;
using UnityEngine;

namespace App.Game.Equipment.External.ViewModel
{
    public class EquipmentSlotsModel
    {
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly EquipmentSlotsView m_SlotsView;

        public EquipmentSlotsModel(EquipmentSlotsView slotsView, ISpriteLoader spriteLoader)
        {
            m_SlotsView = slotsView;
            m_SpriteLoader = spriteLoader;
        }

        //  private readonly InventoryWindow m_Window;
        // private readonly IInventoryConfigController m_ConfigController;
        // private readonly InventorySlotViewCreator m_SlotViewCreator;
        // private readonly InventoryItemsController m_ItemsController;


        public void Initialize()
        {
            
        }

        private void CreateSlots()
        {
            // var rows = m_ConfigController.GetRows();
            // var columns = m_ConfigController.GetCols();
            // var countSlots = columns * rows;
            // m_Slots = new List<ItemSlotModel>(countSlots);
            // for (int i = 0; i < countSlots; ++i)
            // {
            //     var view = m_SlotViewCreator.Create();
            //     if (!view.HasValue)
            //     {
            //         HLogger.LogError($"Failed to create slot view");
            //         return;
            //     }
            //         
            //     var viewModel = new ItemSlotModel(
            //         view.Value,
            //         i,
            //         clickCallback: OnSlotClick,
            //         canPlaceFunc: CanPlace,
            //         placeFunc: Place,
            //         removeFunc: Remove);
            //     viewModel.Initialize();
            //     m_Slots.Add(viewModel);
            // }
        }

        private Optional<IModuleItem> Remove(ItemSlotModel slot)
        {
            return Optional<IModuleItem>.Fail();
            // var item = slot.GetItem();
            // if (item == null)
            // {
            //     HLogger.LogError("Slot is empty");
            //     return Optional<IModuleItem>.Fail();
            // }
            //
            // slot.Clear();
            // m_ItemsController.RemoveItem(item, slot.Index);
            //
            // return Optional<IModuleItem>.Success(item);
        }

        private bool Place(ItemSlotModel slot, IModuleItem item)
        {
            return false;
            // if (slot.HasItem())
            // {
            //     HLogger.LogError($"slot is not empty {slot.GetItem().Id}");
            //     return false;
            // }
            //
            // UpdateSlot(slot, item);
            // m_ItemsController.AddItem(item, slot.Index);
            // return true;
        }

        private bool CanPlace(ItemSlotModel model, IModuleItem item)
        {
            return !model.HasItem();
        }
        
        private void OnSlotClick(ItemSlotModel model)
        {
            EventManager.Trigger(new ItemSlotClickEvent(model));
        }

        public void UpdateSlot(InventoryItem item)
        {
            // var slot = m_Slots[item.Data.Index];
            //
            // UpdateSlot(slot, item.Item);
        }

        private void UpdateSlot(ItemSlotModel slot, IModuleItem item)
        {
            var sprite = GetSprite(item);
            slot.SetItem(item, sprite.Value);
        }

        private Optional<Sprite> GetSprite(IModuleItem item)
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
    }
}