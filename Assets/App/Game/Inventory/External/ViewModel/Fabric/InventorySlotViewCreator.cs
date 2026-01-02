using App.Common.Utilities.Utility.Runtime;
using App.Game.DragItem.Runtime.View;
using App.Game.Inventory.External.View;
using UnityEngine;

namespace App.Game.Inventory.External.ViewModel.Fabric
{
    public class InventorySlotViewCreator
    {
        private readonly InventoryWindow m_Window;

        public InventorySlotViewCreator(InventoryWindow window)
        {
            m_Window = window;
        }

        public Optional<ItemSlotView> Create()
        {
            var view = Object.Instantiate(
                m_Window.ItemSlotViewPrefab,
                m_Window.SlotsContent);
            if (view == null)
            {
                return Optional<ItemSlotView>.Fail();
            }
            
            return Optional<ItemSlotView>.Success(view);
        }
    }
}