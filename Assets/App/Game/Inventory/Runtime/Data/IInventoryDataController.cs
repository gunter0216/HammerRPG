using System.Collections.Generic;

namespace App.Game.Inventory.Runtime.Data
{
    public interface IInventoryDataController
    {
        IReadOnlyList<InventoryItemData> GetItems();
        bool RemoveItem(InventoryItemData itemData);
        bool AddItem(InventoryItemData itemData);
    }
}