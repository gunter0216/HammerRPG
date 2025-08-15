using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.External.View;
using UnityEngine;

namespace App.Game.Inventory.External.Services
{
    public class InventoryGroupViewCreator
    {
        private readonly InventoryWindow m_Window;

        public InventoryGroupViewCreator(InventoryWindow window)
        {
            m_Window = window;
        }

        public Optional<InventoryGroupHeaderView> Create()
        {
            var view = Object.Instantiate(
                m_Window.InventoryGroupHeaderViewPrefab,
                m_Window.HeaderGroupContent);
            if (view == null)
            {
                return Optional<InventoryGroupHeaderView>.Fail();
            }
            
            return Optional<InventoryGroupHeaderView>.Success(view);
        }
    }
}