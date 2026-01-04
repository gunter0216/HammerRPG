using System.Collections.Generic;
using System.Linq;
using App.Common.Events.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.Runtime.Events;
using App.Game.Equipment.External.View;
using App.Game.Equipment.Runtime;
using App.Game.Equipment.Runtime.Items;
using App.Game.GameTiles.External.Config.Model;
using UnityEngine;

namespace App.Game.Equipment.External.ViewModel
{
    public class EquipmentSlotsModel
    {
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly EquipmentSlotsController m_SlotsController;
        private readonly EquipmentSlotsView m_SlotsView;

        private List<EquipmentItemSlotModel> m_Slots;
        private Dictionary<string, EquipmentItemSlotView> m_Views;

        public EquipmentSlotsModel(
            EquipmentSlotsView slotsView,
            ISpriteLoader spriteLoader,
            EquipmentSlotsController slotsController)
        {
            m_SlotsView = slotsView;
            m_SpriteLoader = spriteLoader;
            m_SlotsController = slotsController;
        }

        public void Initialize()
        {
            CreateSlots();

            foreach (var slot in m_Slots)
            {
                UpdateSlot(slot);
            }
        }

        private void CreateSlots()
        {
            m_Slots = new List<EquipmentItemSlotModel>(13);
            CreateSlot(EquipmentSlotConstants.Helmet, m_SlotsView.Helmet);
            CreateSlot(EquipmentSlotConstants.Armor, m_SlotsView.Armor);
            CreateSlot(EquipmentSlotConstants.Shoulder, m_SlotsView.Shoulder);
            CreateSlot(EquipmentSlotConstants.Gloves, m_SlotsView.Gloves);
            CreateSlot(EquipmentSlotConstants.Amulet, m_SlotsView.Amulet);
            CreateSlot(EquipmentSlotConstants.Bracer, m_SlotsView.Bracer);
            CreateSlot(EquipmentSlotConstants.Ring1, m_SlotsView.Ring1);
            CreateSlot(EquipmentSlotConstants.Ring2, m_SlotsView.Ring2);
            CreateSlot(EquipmentSlotConstants.Belt, m_SlotsView.Belt);
            CreateSlot(EquipmentSlotConstants.Pants, m_SlotsView.Pants);
            CreateSlot(EquipmentSlotConstants.Boots, m_SlotsView.Boots);
            CreateSlot(EquipmentSlotConstants.LeftHand, m_SlotsView.LeftHand);
            CreateSlot(EquipmentSlotConstants.RightHand, m_SlotsView.RightHand);
        }

        private void CreateSlot(string type, EquipmentItemSlotView view)
        {
            var slots = m_SlotsController.GetSlots();
            var slot = slots.FirstOrDefault(x => x.Data.SlotKey == type);
            if (slot == default)
            {
                HLogger.LogError($"not found slot {type}");
                return;
            }

            var viewModel = new EquipmentItemSlotModel(
                view,
                slot,
                clickCallback: OnSlotClick,
                canPlaceFunc: CanPlace,
                placeFunc: Place,
                removeFunc: Remove);
            viewModel.Initialize();
            m_Slots.Add(viewModel);
        }

        private Optional<IModuleItem> Remove(EquipmentItemSlotModel slot)
        {
            if (!slot.HasItem())
            {
                HLogger.LogError("Slot is empty");
                return Optional<IModuleItem>.Fail();
            }

            slot.Clear();
            m_SlotsController.RemoveItem(slot.Slot);

            return Optional<IModuleItem>.Success(slot.GetItem());
        }

        private bool Place(EquipmentItemSlotModel slot, IModuleItem item)
        {
            if (slot.HasItem())
            {
                HLogger.LogError($"slot is not empty {slot.GetItem().Id}");
                return false;
            }

            m_SlotsController.AddItem(slot.Slot, item);
            UpdateSlot(slot);

            return true;
        }

        private bool CanPlace(EquipmentItemSlotModel model, IModuleItem item)
        {
            return m_SlotsController.CanPlace(model.Slot, item);
        }

        private void OnSlotClick(EquipmentItemSlotModel model)
        {
            EventManager.Trigger(new ItemSlotClickEvent(model));
        }

        private void UpdateSlot(EquipmentItemSlotModel slot)
        {
            if (slot.Slot.Item == null)
            {
                slot.Clear();
            }
            else
            {
                var sprite = GetSprite(slot.GetItem());
                slot.SetItem(sprite.Value);
            }
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