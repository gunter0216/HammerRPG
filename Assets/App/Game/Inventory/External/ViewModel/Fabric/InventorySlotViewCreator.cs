using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.External.View;
using UnityEngine;

namespace App.Game.Inventory.External.Services
{
    public class InventorySlotViewCreator
    {
        private readonly InventoryWindow m_Window;

        public InventorySlotViewCreator(InventoryWindow window)
        {
            m_Window = window;
        }

        public Optional<InventorySlotView> Create()
        {
            var view = Object.Instantiate(
                m_Window.InventorySlotViewPrefab,
                m_Window.SlotsContent);
            if (view == null)
            {
                return Optional<InventorySlotView>.Fail();
            }
            
            return Optional<InventorySlotView>.Success(view);
        }
    }
}