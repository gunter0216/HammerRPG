using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.External.View;
using UnityEngine;

namespace App.Game.Inventory.External.Services
{
    public class InventoryItemViewCreator
    {
        private readonly InventoryWindow m_InventoryWindow;
        
        public InventoryItemViewCreator(InventoryWindow inventoryWindow)
        {
            m_InventoryWindow = inventoryWindow;
        }

        public Optional<InventoryItemView> Create()
        {
            var itemView = Object.Instantiate(
                m_InventoryWindow.InventoryItemViewPrefab, 
                m_InventoryWindow.ItemsContent);
            if (itemView == null)
            {
                HLogger.LogError("Failed to instantiate InventoryItemViewPrefab.");
                return Optional<InventoryItemView>.Empty;
            }
            
            return Optional<InventoryItemView>.Success(itemView);
        }
    }
}