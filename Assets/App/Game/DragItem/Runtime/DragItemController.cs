using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Events.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.DragItem.Runtime.Events;
using App.Game.DragItem.Runtime.Model;
using App.Game.DragItem.Runtime.View;
using App.Game.Modules.ContainerItem.Runtime;
using App.Game.Modules.Equipment.Runtime;
using App.Game.Modules.Equipment.Runtime.Config;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace App.Game.DragItem.Runtime
{
    public class DragItemController : IInitSystem, IDisposable
    {
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvasController _canvasController;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private ItemViewModel m_DragView;
        private IItemSlotModel m_SourceSlot;
        private IModuleItem _draggedItem;
        private MonoBehaviour _gestureSource;
        private int? _gesturePointerId;
        private ItemSlotController _previewSlot;
        private PointerEventData _pointerData;
        private EventSystem _eventSystem;
        private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();
        
        private IDisposable m_Disposable;

        public bool IsDragging => _draggedItem != null;

        public void CancelDragFrom(IItemSlotModel slot)
        {
            if (m_SourceSlot != null && m_SourceSlot == slot)
                FinishDrag();
        }

        public bool CanEquipDraggedItem(EquipmentModule equipment, EquipmentSlotId slot)
        {
            return HasDraggedGridItem() && equipment.CanPlaceItem(slot, _draggedItem);
        }

        public bool TryEquipDraggedItem(EquipmentModule equipment, EquipmentSlotId slot)
        {
            if (!HasDraggedGridItem() || !equipment.TryEquip(slot, _draggedItem))
                return false;

            FinishDrag();
            return true;
        }

        private bool HasDraggedGridItem()
        {
            return IsDragging && m_SourceSlot is ItemSlotController source &&
                   source.Container != null && source.GetItem() == _draggedItem;
        }

        public DragItemController(IAssetManager assetManager, ICanvasController canvasController, ISpriteLoader spriteLoader)
        {
            m_AssetManager = assetManager;
            _canvasController = canvasController;
            m_SpriteLoader = spriteLoader;
        }

        public void Init()
        {
            if (!CreateView())
            {
                HLogger.LogError("Could not create the dragged item view.");
                return;
            }
            
            EventManager.Subscribe<ItemSlotClickEvent>(OnSlotClick);
            EventManager.Subscribe<ItemSlotDragStartedEvent>(OnSlotDragStarted);
            EventManager.Subscribe<ItemSlotDragEndedEvent>(OnSlotDragEnded);
            
            m_Disposable = Observable.EveryUpdate()
                .Where(_ => m_DragView != null && m_DragView.IsActive())
                .Subscribe(_ => UpdateDragPosition());
        }

        private void UpdateDragPosition()
        {
            if (m_SourceSlot == null || m_SourceSlot.GetItem() != _draggedItem ||
                (_gesturePointerId.HasValue && (_gestureSource == null || !_gestureSource.isActiveAndEnabled)))
            {
                FinishDrag();
                return;
            }

            var mouse = Mouse.current;
            if (mouse == null)
            {
                SetPreviewSlot(null);
                return;
            }

            var position = mouse.position.ReadValue();
            if (!m_DragView.TrySetScreenPosition(position))
                HLogger.LogError("Could not position the dragged item on its canvas.");
            UpdateDropPreview(position);
        }

        private void UpdateDropPreview(Vector2 position)
        {
            var events = EventSystem.current;
            ItemSlotController target = null;
            if (events != null)
            {
                if (_eventSystem != events || _pointerData == null)
                {
                    _eventSystem = events;
                    _pointerData = new PointerEventData(events);
                }
                _pointerData.position = position;
                _raycastResults.Clear();
                events.RaycastAll(_pointerData, _raycastResults);
                if (_raycastResults.Count > 0)
                {
                    var view = _raycastResults[0].gameObject.GetComponentInParent<ItemSlotView>();
                    if (view != null && view.Model != null)
                        target = ResolveGridDropTarget(view.Model, position);
                }
            }

            if (target != null)
            {
                bool canDrop = CanDropInGrid(target);
                if (!canDrop)
                    target = null;
            }
            SetPreviewSlot(target);
        }

        private bool CanDropInGrid(ItemSlotController target)
        {
            if (m_SourceSlot is EquippedItemSlotModel equipment)
                return equipment.CanMoveToInventory(target);
            if (m_SourceSlot is ItemSlotController source && source.Container != null && source.Container == target.Container)
                return source.Container.CanMoveItem(source.Index, target.Index);

            return CanTransferItem(target, target.GetItem());
        }

        private void SetPreviewSlot(ItemSlotController slot)
        {
            if (_previewSlot == slot)
                return;
            _previewSlot?.HideDropPreview();
            _previewSlot = slot;
            _previewSlot?.ShowDropPreview(_draggedItem);
        }

        private ItemSlotController ResolveGridDropTarget(ItemSlotController hovered, Vector2? position)
        {
            if (hovered == m_SourceSlot)
                return hovered;

            var target = hovered;
            if (_draggedItem.TryGetModule<ContainerItemModule>(out var size) &&
                (size.Width > 1 || size.Height > 1))
                target = hovered.ResolveDrop(_draggedItem, position);
            if (target == null)
                return null;

            var anchor = target.ResolveAnchor();
            return anchor.GetItem() == _draggedItem ? target : anchor;
        }

        private bool CreateView()
        {
            var creator = new DragItemViewCreator(m_AssetManager, _canvasController);
            var view = creator.Create();
            if (!view.HasValue)
                return false;
            m_DragView = new ItemViewModel(view.Value, m_SpriteLoader);
            m_DragView.SetActive(false);
            return true;
        }

        private void OnSlotClick(ItemSlotClickEvent data)
        {
            if (_gesturePointerId.HasValue)
                return;

            if (m_SourceSlot != null && m_SourceSlot.GetItem() != _draggedItem)
                FinishDrag();

            if (m_SourceSlot == null)
            {
                if (TryPickUpItem(data.Model))
                    UpdateDragPosition();
                return;
            }

            DropOnSlot(data.Model, Mouse.current?.position.ReadValue());
        }

        private void OnSlotDragStarted(ItemSlotDragStartedEvent data)
        {
            if (IsDragging || !TryPickUpItem(data.Model))
                return;

            _gestureSource = data.Source;
            _gesturePointerId = data.PointerId;
            UpdateDragPosition();
        }

        private void OnSlotDragEnded(ItemSlotDragEndedEvent data)
        {
            if (_gestureSource != data.Source || _gesturePointerId != data.PointerId)
                return;

            if (data.Target != null && m_SourceSlot.GetItem() == _draggedItem)
                DropOnSlot(data.Target, data.Position);

            if (IsDragging)
                FinishDrag();
        }

        private bool TryPickUpItem(IItemSlotModel slot)
        {
            if (slot is ItemSlotController gridSlot)
                slot = gridSlot.ResolveAnchor();

            var item = slot.GetItem();
            if (item == null)
                return false;

            m_SourceSlot = slot;
            _draggedItem = item;
            slot.UpItem();
            var icon = slot switch
            {
                ItemSlotController grid => grid.Icon,
                EquippedItemSlotModel equipment => equipment.Icon,
                _ => null
            };
            m_DragView.SetItem(item, icon);
            m_DragView.SetActive(true);
            return true;
        }

        private void DropOnSlot(IItemSlotModel targetSlot, Vector2? position)
        {
            if (targetSlot == m_SourceSlot)
            {
                FinishDrag();
                return;
            }

            if (targetSlot is ItemSlotController dropSlot)
            {
                targetSlot = ResolveGridDropTarget(dropSlot, position);
                if (targetSlot == null)
                    return;
            }

            if (m_SourceSlot is EquippedItemSlotModel equipment)
            {
                if (targetSlot is ItemSlotController inventorySlot && equipment.TryMoveToInventory(inventorySlot))
                    FinishDrag();
                return;
            }

            if (m_SourceSlot is ItemSlotController sourceGridSlot && targetSlot is ItemSlotController targetGridSlot &&
                sourceGridSlot.Container != null && sourceGridSlot.Container == targetGridSlot.Container)
            {
                if (sourceGridSlot.Container.TryMoveItem(sourceGridSlot.Index, targetGridSlot.Index))
                    FinishDrag();
                return;
            }

            if (targetSlot is ItemSlotController gridSlot)
                targetSlot = gridSlot.ResolveAnchor();

            if (TryTransferItem(targetSlot, targetSlot.GetItem()))
                FinishDrag();
        }

        private bool TryTransferItem(IItemSlotModel targetSlot, IModuleItem targetItem)
        {
            if (!CanTransferItem(targetSlot, targetItem) ||
                !m_SourceSlot.RemoveItem().HasValue)
                return false;

            if (targetItem != null && !targetSlot.RemoveItem().HasValue)
            {
                RestoreItem(m_SourceSlot, _draggedItem);
                return false;
            }

            if (targetSlot.PlaceItem(_draggedItem))
            {
                if (targetItem == null || m_SourceSlot.PlaceItem(targetItem))
                    return true;
                if (!targetSlot.RemoveItem().HasValue)
                {
                    HLogger.LogError("Could not undo the item transfer.");
                    return false;
                }
            }

            RestoreItem(m_SourceSlot, _draggedItem);
            if (targetItem != null)
                RestoreItem(targetSlot, targetItem);
            return false;
        }

        private bool CanTransferItem(IItemSlotModel targetSlot, IModuleItem targetItem)
        {
            return targetSlot.CanPlaceItem(_draggedItem) &&
                (targetItem == null || m_SourceSlot.CanPlaceItem(targetItem));
        }

        private static void RestoreItem(IItemSlotModel slot, IModuleItem item)
        {
            if (!slot.PlaceItem(item))
                HLogger.LogError("Could not restore the item to its original slot.");
        }

        private void FinishDrag()
        {
            SetPreviewSlot(null);
            m_SourceSlot?.DownItem();
            m_DragView.SetActive(false);
            m_SourceSlot = null;
            _draggedItem = null;
            _gestureSource = null;
            _gesturePointerId = null;
        }

        public void Dispose()
        {
            SetPreviewSlot(null);
            m_Disposable?.Dispose();
            EventManager.Unsubscribe<ItemSlotClickEvent>(OnSlotClick);
            EventManager.Unsubscribe<ItemSlotDragStartedEvent>(OnSlotDragStarted);
            EventManager.Unsubscribe<ItemSlotDragEndedEvent>(OnSlotDragEnded);
        }
    }
}
