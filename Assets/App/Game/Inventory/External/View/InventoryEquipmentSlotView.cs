using App.Common.Events.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.DragItem.Runtime;
using App.Game.DragItem.Runtime.Events;
using App.Game.DragItem.Runtime.Model;
using App.Game.DragItem.Runtime.View;
using App.Game.Modules.ContainerItem.Runtime;
using App.Game.Modules.Equipment.Runtime;
using App.Game.Modules.Equipment.Runtime.Config;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Game.Inventory.External.View
{
    public sealed class InventoryEquipmentSlotView : MonoBehaviour, IPointerClickHandler, IDropHandler,
        IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private EquipmentSlotId _slot;
        [SerializeField] private UnityEngine.UI.Image _placeholder;
        [SerializeField] private UnityEngine.UI.Image _itemImage;
        [SerializeField] private UnityEngine.UI.Image _dropPreview;

        private EquipmentModule _equipment;
        private DragItemController _dragItemController;
        private IItemSpriteLoader _spriteLoader;
        private UnityEngine.UI.GridLayoutGroup _inventoryGrid;
        private EquippedItemSlotModel _model;
        private bool _pointerOver;

        public IModuleItem Item => _model?.GetItem();

        public void Bind(EquipmentModule equipment, DragItemController dragItemController, IItemSpriteLoader spriteLoader,
            UnityEngine.UI.GridLayoutGroup inventoryGrid)
        {
            Unbind();
            _equipment = equipment;
            _dragItemController = dragItemController;
            _spriteLoader = spriteLoader;
            _inventoryGrid = inventoryGrid;
            _model = new EquippedItemSlotModel(equipment, _slot, _itemImage);
            _equipment.SlotChanged += OnSlotChanged;
            UpdateItem();
        }

        public void Unbind()
        {
            _dragItemController?.CancelDragFrom(_model);
            if (_equipment != null)
                _equipment.SlotChanged -= OnSlotChanged;

            _equipment = null;
            _model = null;
            SetPreview(false);
        }

        private void OnSlotChanged(EquipmentSlotId slot)
        {
            if (slot == _slot)
                UpdateItem();
        }

        private void UpdateItem()
        {
            Sprite sprite = null;
            if (_equipment.TryGetEquippedItem(_slot, out var item))
            {
                var loaded = _spriteLoader.LoadItemSprite(item);
                if (loaded.HasValue)
                {
                    sprite = loaded.Value;
                    var hasFootprint = item.TryGetModule<ContainerItemModule>(out var footprint);
                    int width = hasFootprint ? footprint.Width : 1;
                    int height = hasFootprint ? footprint.Height : 1;
                    ItemSlotView.SetCenteredIconFootprint(_itemImage, _inventoryGrid, width, height);
                }
            }

            _itemImage.sprite = sprite;
            _itemImage.gameObject.SetActive(sprite != null);
            _placeholder.gameObject.SetActive(sprite == null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!Handles(eventData))
                return;

            if (!_dragItemController.IsDragging)
            {
                EventManager.Trigger(new ItemSlotClickEvent(_model));
                return;
            }

            EquipDraggedItem();
            _dragItemController.CancelDragFrom(_model);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (Handles(eventData))
                EquipDraggedItem();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!Handles(eventData))
                return;

            eventData.eligibleForClick = false;
            EventManager.Trigger(new ItemSlotDragStartedEvent(this, _model, eventData.pointerId));
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Handles(eventData))
                EventManager.Trigger(new ItemSlotDragEndedEvent(this, eventData));
        }

        private bool Handles(PointerEventData eventData) =>
            _model != null && eventData.button == PointerEventData.InputButton.Left;

        private void EquipDraggedItem()
        {
            if (_dragItemController.TryEquipDraggedItem(_equipment, _slot))
                SetPreview(false);
        }

        public void OnPointerEnter(PointerEventData eventData) => _pointerOver = true;

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerOver = false;
            SetPreview(false);
        }

        private void LateUpdate()
        {
            if (_pointerOver && _equipment != null)
                SetPreview(_dragItemController.CanEquipDraggedItem(_equipment, _slot));
        }

        private void OnDisable()
        {
            _pointerOver = false;
            SetPreview(false);
            _dragItemController?.CancelDragFrom(_model);
        }

        private void OnDestroy() => Unbind();

        private void SetPreview(bool active)
        {
            if (_dropPreview != null && _dropPreview.gameObject.activeSelf != active)
                _dropPreview.gameObject.SetActive(active);
        }
    }
}
