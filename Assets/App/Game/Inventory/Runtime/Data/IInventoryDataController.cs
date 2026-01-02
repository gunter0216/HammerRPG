using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Game.Inventory.Runtime.Data.Model;

namespace App.Game.Inventory.Runtime.Data
{
    public interface IInventoryDataController
    {
        IReadOnlyList<InventoryGroupData> GetGroups();
        bool AddGroup(InventoryGroupData groupData);
        bool RemoveItem(string group, int index);
        // bool AddItem(InventoryItemData itemData);
        void SetItem(string group, InventoryItemData data);
    }
}