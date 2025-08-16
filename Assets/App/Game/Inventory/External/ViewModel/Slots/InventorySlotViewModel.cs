using System;
using App.Game.Inventory.External.View;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventorySlotViewModel
    {
        private readonly InventorySlotView m_View;
        private readonly int m_Row;
        private readonly int m_Col;
        
        private InventoryItemViewModel m_Item;
        private event Action<InventorySlotViewModel> m_ClickCallback;

        public InventoryItemViewModel Item => m_Item;
        public int Row => m_Row;
        public int Col => m_Col;

        public InventorySlotViewModel(
            InventorySlotView view, 
            int row, 
            int col,
            Action<InventorySlotViewModel> clickCallback)
        {
            m_View = view;
            m_Row = row;
            m_Col = col;
            m_ClickCallback = clickCallback;
        }

        public void Initialize()
        {
            m_View.SetButtonClickCallback(OnButtonClick);
        }

        public void SetItem(InventoryItemViewModel item)
        {
            m_Item = item;
        }
        
        private void OnButtonClick()
        {
            m_ClickCallback?.Invoke(this);
        }
        
        public Vector2 GetLocalPosition()
        {
            return m_View.GetLocalPosition();
        }
    }
}