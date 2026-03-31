using System;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.Runtime.View;
using UnityEngine;

namespace App.Game.DragItem.Runtime.Model
{
    public class ItemSlotController : IItemSlotModel
    {
        private readonly ItemSlotView m_View;
        private int m_Index;

        private readonly Func<ItemSlotController, IModuleItem, bool> m_CanPlaceFunc;
        private readonly Func<ItemSlotController, IModuleItem, bool> m_PlaceFunc;
        private readonly Func<ItemSlotController, Optional<IModuleItem>> m_RemoveFunc;
        private event Action<ItemSlotController> m_ClickCallback;

        private IModuleItem m_Item;

        public int Index => m_Index;

        public ItemSlotController(
            ItemSlotView view, 
            int index,
            Action<ItemSlotController> clickCallback, 
            Func<ItemSlotController, IModuleItem, bool> canPlaceFunc, 
            Func<ItemSlotController, IModuleItem, bool> placeFunc, 
            Func<ItemSlotController, Optional<IModuleItem>> removeFunc)
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

        public void SetIndex(int index)
        {
            m_Index = index;
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
            m_View.SetItemActive(sprite != null);
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

        public void SetActive(bool status)
        {
            m_View.gameObject.SetActive(status);
        }
    }
}