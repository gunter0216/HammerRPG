using App.Game.Inventory.Runtime.Data.Model;

namespace App.Game.Inventory.Runtime.Data
{
    public interface IInventoryDataService
    {
        bool RemoveItem(int index);
        void AddItem(InventoryItemData data);
    }
}