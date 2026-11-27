using App.Game.DragItem.Runtime.View;
using System;
using App.Common.ModuleItem.Runtime;
using App.Game.DragItem.Runtime;
using App.Game.DragItem.Runtime.Model;
using App.Game.Inventory.External.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game.Containers.ContainerWindow.External.View
{
    public class ContainerWindow : MonoBehaviour
    {
        [SerializeField] private ItemSlotView m_ItemSlotViewPrefab;
        [SerializeField] private Transform m_SlotsContent;
        [SerializeField] private TMP_Text _nameText;
        [Space]
        [SerializeField] private Button m_CloseButton;
        [SerializeField] private InventoryItemInfoView _itemInfoPrefab;

        private DragItemController _drag;
        private Func<IModuleItem, IModuleItem> _findEquippedItem;
        private InventoryItemInfoView _itemInfo;
        private InventoryItemInfoView _equippedInfo;
        private ItemSlotController _hoveredSlot;
        private IModuleItem _hoveredItem;
        private IModuleItem _equippedItem;
        
        public ItemSlotView ItemSlotViewPrefab => m_ItemSlotViewPrefab;
        public Transform SlotsContent => m_SlotsContent;

        internal bool InitializeItemHover(DragItemController drag, Func<IModuleItem, IModuleItem> findEquippedItem)
        {
            if (_itemInfoPrefab == null || drag == null || findEquippedItem == null || transform is not RectTransform viewport)
                return false;

            _drag = drag;
            _findEquippedItem = findEquippedItem;
            _itemInfo = Instantiate(_itemInfoPrefab, viewport);
            _itemInfo.Initialize(viewport);
            _equippedInfo = Instantiate(_itemInfoPrefab, viewport);
            _equippedInfo.Initialize(viewport);
            return true;
        }

        private void LateUpdate()
        {
            if (_itemInfo == null)
                return;

            var hit = _drag.GetHoveredObject();
            var cell = hit != null ? hit.GetComponentInParent<ItemSlotView>() : null;
            var slot = cell != null && cell.transform.parent == m_SlotsContent ? cell.Model?.ResolveAnchor() : null;
            var item = slot?.GetItem();
            if (item == null || !slot.Icon.isActiveAndEnabled)
            {
                ClearItemHover();
                return;
            }

            if (_hoveredSlot != slot || _hoveredItem != item)
            {
                _hoveredSlot?.SetHovered(false);
                _hoveredSlot = slot;
                _hoveredItem = item;
                slot.SetHovered(true);
                _itemInfo.SetItem(item, false);
            }

            var equipped = _findEquippedItem(item);
            if (equipped != _equippedItem)
            {
                _equippedItem = equipped;
                if (equipped != null)
                    _equippedInfo.SetItem(equipped, true);
                else
                    _equippedInfo.Hide();
            }

            if (!_itemInfo.TryPositionRightOf(slot.Icon.rectTransform, equipped != null ? _equippedInfo : null))
            {
                _itemInfo.Hide();
                _equippedInfo.Hide();
            }
        }

        private void OnDisable() => ClearItemHover();

        private void ClearItemHover()
        {
            _hoveredSlot?.SetHovered(false);
            _hoveredSlot = null;
            _hoveredItem = null;
            _equippedItem = null;
            if (_itemInfo != null)
                _itemInfo.Hide();
            if (_equippedInfo != null)
                _equippedInfo.Hide();
        }

        public void SetColumns(int columns)
        {
            m_SlotsContent.GetComponent<GridLayoutGroup>().constraintCount = columns;
        }
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }

        public void SetName(string text)
        {
            _nameText.text = text;
        }
        
        public void SetCloseButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(callback);
        }
    }
}
