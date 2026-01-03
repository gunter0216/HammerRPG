using System;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.Runtime.Model;
using App.Game.Equipment.External.View;
using App.Game.Equipment.Runtime.Item;
using UnityEngine;

namespace App.Game.Equipment.External.ViewModel
{
    public class EquipmentItemSlotModel : IItemSlotModel
    {
        private readonly EquipmentItemSlotView m_View;
        private readonly EquipmentSlot m_Slot;

        private readonly Func<EquipmentItemSlotModel, IModuleItem, bool> m_CanPlaceFunc;
        private readonly Func<EquipmentItemSlotModel, IModuleItem, bool> m_PlaceFunc;
        private readonly Func<EquipmentItemSlotModel, Optional<IModuleItem>> m_RemoveFunc;
        private event Action<EquipmentItemSlotModel> m_ClickCallback;

        public EquipmentSlot Slot => m_Slot;

        public EquipmentItemSlotModel(
            EquipmentItemSlotView view,
            EquipmentSlot slot,
            Action<EquipmentItemSlotModel> clickCallback,
            Func<EquipmentItemSlotModel, IModuleItem, bool> canPlaceFunc,
            Func<EquipmentItemSlotModel, IModuleItem, bool> placeFunc,
            Func<EquipmentItemSlotModel, Optional<IModuleItem>> removeFunc)
        {
            m_View = view;
            m_Slot = slot;
            m_CanPlaceFunc = canPlaceFunc;
            m_PlaceFunc = placeFunc;
            m_RemoveFunc = removeFunc;
            m_ClickCallback = clickCallback;
        }

        public void Initialize()
        {
            m_View.SetButtonClickCallback(OnButtonClick);
        }

        public void Clear()
        {
            m_View.SetItemActive(false);
        }
        
        private void OnButtonClick()
        {
            m_ClickCallback?.Invoke(this);
        }
        
        public IModuleItem GetItem()
        {
            return Slot.Item;
        }

        public bool HasItem()
        {
            return GetItem() != null;
        }
        
        public bool CanPlaceItem(IModuleItem item)
        {
            return m_CanPlaceFunc(this, item);
        }

        public bool PlaceItem(IModuleItem item)
        {
            return m_PlaceFunc(this, item);
        }

        public Optional<IModuleItem> RemoveItem()
        {
            return m_RemoveFunc(this);
        }

        public void UpItem()
        {
            m_View.SetPickupState(true);
        }

        public void DownItem()
        {
            m_View.SetPickupState(false);
        }

        public void SetItem(Sprite sprite)
        {
            m_View.SetSprite(sprite);
            m_View.SetItemActive(true);
        }
    }
}