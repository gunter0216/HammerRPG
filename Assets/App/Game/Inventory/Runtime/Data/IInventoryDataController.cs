using System.Collections.Generic;

namespace App.Game.Inventory.Runtime.Data
{
    public interface IInventoryDataController
    {
        List<InventoryItemData> GetItems();
    }
}