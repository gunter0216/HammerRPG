using UnityEngine;
using UnityEngine.Events;

namespace App.Game.DragItem.Runtime.View
{
    public interface IItemSlotView
    {
        void SetItemActive(bool status);
        void SetSprite(Sprite sprite);
        void SetPickupState(bool status);
        void SetButtonClickCallback(UnityAction callback);
    }
}