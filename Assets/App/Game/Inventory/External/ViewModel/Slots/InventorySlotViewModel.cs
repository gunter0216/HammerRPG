using App.Game.Inventory.External.View;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventorySlotViewModel
    {
        private readonly InventorySlotView m_View;

        public InventorySlotViewModel(InventorySlotView view)
        {
            m_View = view;
        }
        
        public Vector2 GetLocalPosition()
        {
            return m_View.GetLocalPosition();
        }
    }
}