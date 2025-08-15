using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Inventory.External.AddItemStrategy
{
    public interface IInventoryAddItemStrategy
    {
        bool AddItem(IModuleItemConfig moduleItemConfig);
        bool AddItem(string id);
        bool AddItem(IModuleItem moduleItem);
    }
}