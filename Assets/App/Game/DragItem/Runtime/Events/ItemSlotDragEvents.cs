using App.Game.DragItem.Runtime.Model;
using App.Game.DragItem.Runtime.View;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Game.DragItem.Runtime.Events
{
    public readonly struct ItemSlotDragStartedEvent
    {
        public MonoBehaviour Source { get; }
        public IItemSlotModel Model { get; }
        public int PointerId { get; }

        public ItemSlotDragStartedEvent(MonoBehaviour source, IItemSlotModel model, int pointerId)
        {
            Source = source;
            Model = model;
            PointerId = pointerId;
        }
    }

    public readonly struct ItemSlotDragEndedEvent
    {
        public MonoBehaviour Source { get; }
        public int PointerId { get; }
        public ItemSlotController Target { get; }
        public Vector2 Position { get; }

        public ItemSlotDragEndedEvent(MonoBehaviour source, PointerEventData eventData)
        {
            var hit = eventData.pointerCurrentRaycast.gameObject;
            var target = hit != null ? hit.GetComponentInParent<ItemSlotView>() : null;
            Source = source;
            PointerId = eventData.pointerId;
            Target = target != null ? target.Model : null;
            Position = eventData.position;
        }
    }
}
