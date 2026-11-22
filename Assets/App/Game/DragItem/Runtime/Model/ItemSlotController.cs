using System;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.Runtime.View;
using App.Game.Modules.ContainerItem.Runtime;
using UnityEngine;
using ItemContainer = App.Game.Containers.Container.Runtime.Container;

namespace App.Game.DragItem.Runtime.Model
{
    public class ItemSlotController : IItemSlotModel
    {
        private readonly ItemSlotView m_View;
        private int m_Index;

        private readonly Func<ItemSlotController, IModuleItem, bool> m_CanPlaceFunc;
        private readonly Func<ItemSlotController, IModuleItem, bool> m_PlaceFunc;
        private readonly Func<ItemSlotController, Optional<IModuleItem>> m_RemoveFunc;
        private readonly Func<ItemContainer> _getContainer;
        private readonly Func<int, ItemSlotController> _getSlot;
        private event Action<ItemSlotController> m_ClickCallback;

        private IModuleItem m_Item;

        public int Index => m_Index;
        public ItemContainer Container => _getContainer();
        public UnityEngine.UI.Image Icon => m_View.Icon;

        public ItemSlotController(
            ItemSlotView view, 
            int index,
            Action<ItemSlotController> clickCallback, 
            Func<ItemSlotController, IModuleItem, bool> canPlaceFunc, 
            Func<ItemSlotController, IModuleItem, bool> placeFunc, 
            Func<ItemSlotController, Optional<IModuleItem>> removeFunc,
            Func<ItemContainer> getContainer,
            Func<int, ItemSlotController> getSlot)
        {
            m_View = view;
            m_CanPlaceFunc = canPlaceFunc;
            m_PlaceFunc = placeFunc;
            m_RemoveFunc = removeFunc;
            _getContainer = getContainer;
            _getSlot = getSlot;
            m_Index = index;
            m_ClickCallback = clickCallback;
        }

        public void Initialize()
        {
            m_View.Bind(this);
            m_View.SetButtonClickCallback(OnButtonClick);
        }

        public void ShowDropPreview(IModuleItem item)
        {
            if (item != null && item.TryGetModule<ContainerItemModule>(out var size))
                m_View.SetDropPreview(true, size.Width, size.Height);
        }

        public void HideDropPreview()
        {
            m_View.SetDropPreview(false);
        }

        public void SetHovered(bool hovered)
        {
            m_View.SetItemHovered(hovered);
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
            if (item != null && item.TryGetModule<ContainerItemModule>(out var size))
                m_View.SetFootprint(size.Width, size.Height);
            else
                m_View.SetFootprint(1, 1);
            m_View.SetSprite(sprite);
            m_View.SetItemActive(sprite != null);
        }

        public ItemSlotController ResolveAnchor()
        {
            int anchor = Container?.GetAnchorIndex(Index) ?? -1;
            return anchor >= 0 && anchor != Index ? _getSlot(anchor) : this;
        }

        public ItemSlotController ResolveDrop(IModuleItem item, Vector2? screenPoint)
        {
            Vector2 offset = screenPoint.HasValue ? m_View.GetCellPointerOffset(screenPoint.Value) : Vector2.zero;
            int anchor = Container?.FindDropIndex(item, Index, offset.x, offset.y) ?? -1;
            if (anchor < 0)
                return null;
            return anchor == Index ? this : _getSlot(anchor);
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
            if (!status)
            {
                Clear();
                DownItem();
            }
            m_View.gameObject.SetActive(status);
        }
        
        public void SetAsLastSibling()
        {
            m_View.SetAsLastSibling();
        }
    }
}
