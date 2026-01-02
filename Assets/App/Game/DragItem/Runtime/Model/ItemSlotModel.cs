using System;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.External.Model;
using App.Game.Inventory.External.View;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel
{
    public class ItemSlotModel : IItemSlotModel
    {
        private readonly ItemSlotView m_View;
        private readonly int m_Index;

        private readonly Func<ItemSlotModel, IModuleItem, bool> m_CanPlaceFunc;
        private readonly Func<ItemSlotModel, IModuleItem, bool> m_PlaceFunc;
        private readonly Func<ItemSlotModel, Optional<IModuleItem>> m_RemoveFunc;
        private event Action<ItemSlotModel> m_ClickCallback;

        private IModuleItem m_Item;

        public int Index => m_Index;

        public ItemSlotModel(
            ItemSlotView view, 
            int index,
            Action<ItemSlotModel> clickCallback, 
            Func<ItemSlotModel, IModuleItem, bool> canPlaceFunc, 
            Func<ItemSlotModel, IModuleItem, bool> placeFunc, 
            Func<ItemSlotModel, Optional<IModuleItem>> removeFunc)
        {
            m_View = view;
            m_CanPlaceFunc = canPlaceFunc;
            m_PlaceFunc = placeFunc;
            m_RemoveFunc = removeFunc;
            m_Index = index;
            m_ClickCallback = clickCallback;
        }

        public void Initialize()
        {
            m_View.SetButtonClickCallback(OnButtonClick);
        }

        public void Clear()
        {
            m_Item = null;
            m_View.SetItemActive(false);
        }

        public void SetItem(IModuleItem item, Sprite sprite)
        {
            m_Item = item;
            m_View.SetSprite(sprite);
            m_View.SetItemActive(true);
        }
        
        private void OnButtonClick()
        {
            m_ClickCallback?.Invoke(this);
        }
        
        public IModuleItem GetItem()
        {
            return m_Item;
        }

        public bool HasItem()
        {
            return m_Item != null;
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
    }
}