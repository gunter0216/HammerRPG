using App.Game.DragItem.Runtime.View;
using App.Common.ModuleItem.Runtime;
using App.Game.DragItem.Runtime;
using App.Game.DragItem.Runtime.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace App.Game.Inventory.External.View
{
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField] private ItemSlotView _itemSlotViewPrefab;
        [SerializeField] private Transform _slotsContent;
        [Space]
        [SerializeField] private Transform _itemsContent;
        [SerializeField] private Button _closeButton;
        [SerializeField] private InventoryItemInfoView _itemInfoPrefab;
        [SerializeField] private InventoryEquipmentSlotView[] _equipmentSlots;

        private DragItemController _dragItemController;
        private InventoryItemInfoView _itemInfo;
        private ItemSlotController _hoveredSlot;
        private IModuleItem _hoveredItem;
        private bool _hoveredEquipped;
        
        public ItemSlotView ItemSlotViewPrefab => _itemSlotViewPrefab;
        public Transform SlotsContent => _slotsContent;
        public Transform ItemsContent => _itemsContent;
        public IReadOnlyList<InventoryEquipmentSlotView> EquipmentSlots => _equipmentSlots;

        public bool InitializeItemHover(DragItemController dragItemController)
        {
            if (_itemInfoPrefab == null || dragItemController == null || transform is not RectTransform viewport)
                return false;

            _dragItemController = dragItemController;
            _itemInfo = Instantiate(_itemInfoPrefab, viewport);
            _itemInfo.Initialize(viewport);
            return true;
        }

        private void LateUpdate()
        {
            if (_itemInfo == null)
                return;

            var hit = FindHoveredObject();
            var slotView = hit != null ? hit.GetComponentInParent<ItemSlotView>() : null;
            var slot = slotView != null && slotView.transform.parent == _slotsContent
                ? slotView.Model?.ResolveAnchor()
                : null;
            var equippedSlot = slot == null && hit != null ? hit.GetComponentInParent<InventoryEquipmentSlotView>() : null;
            if (equippedSlot != null && !equippedSlot.transform.IsChildOf(transform))
                equippedSlot = null;

            bool equipped = equippedSlot != null;
            var item = equipped ? equippedSlot.Item : slot?.GetItem();
            if (item == null || (!equipped && !slot.Icon.isActiveAndEnabled))
            {
                ClearItemHover();
                return;
            }

            if (_hoveredSlot != slot || _hoveredItem != item || _hoveredEquipped != equipped)
            {
                _hoveredSlot?.SetHovered(false);
                _hoveredSlot = slot;
                _hoveredItem = item;
                _hoveredEquipped = equipped;
                slot?.SetHovered(true);
                _itemInfo.SetItem(item, equipped);
            }

            var footprint = equipped ? (RectTransform)equippedSlot.transform : slot.Icon.rectTransform;
            if (!_itemInfo.TryPositionLeftOf(footprint))
                _itemInfo.Hide();
        }

        private GameObject FindHoveredObject()
        {
            var mouse = Mouse.current;
            if (_dragItemController.IsDragging || mouse == null ||
                EventSystem.current?.currentInputModule is not InputSystemUIInputModule inputModule)
                return null;

            return inputModule.GetLastRaycastResult(mouse.deviceId).gameObject;
        }

        private void OnDisable()
        {
            ClearItemHover();
        }

        private void ClearItemHover()
        {
            _hoveredSlot?.SetHovered(false);
            _hoveredSlot = null;
            _hoveredItem = null;
            _hoveredEquipped = false;
            if (_itemInfo != null)
                _itemInfo.Hide();
        }
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
        
        public void SetCloseButtonClickCallback(UnityEngine.Events.UnityAction callback)
        {
            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(callback);
        }
    }
}
