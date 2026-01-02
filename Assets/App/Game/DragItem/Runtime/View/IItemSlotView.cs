using UnityEngine;
using UnityEngine.Events;

namespace App.Game.Inventory.External.View
{
    public interface IItemSlotView
    {
        void SetItemActive(bool status);
        void SetSprite(Sprite sprite);
        void SetPickupState(bool status);
        void SetButtonClickCallback(UnityAction callback);
    }
}