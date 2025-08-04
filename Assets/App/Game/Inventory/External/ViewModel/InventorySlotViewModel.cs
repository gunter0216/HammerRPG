using App.Game.Inventory.External.View;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventorySlotViewModel
    {
        private readonly InventorySlotView m_View;

        public InventorySlotViewModel(InventorySlotView view)
        {
            m_View = view;
        }
    }
}