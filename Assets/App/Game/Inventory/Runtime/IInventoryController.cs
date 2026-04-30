using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Inventory.Runtime
{
    public interface IInventoryController
    {
        void OpenWindow();
        void CloseWindow();
        bool IsOpen();
        bool AddItem(IModuleItemConfig moduleItemConfig);
        bool AddItem(string id);
        bool AddItem(IModuleItem moduleItem);
    }
}