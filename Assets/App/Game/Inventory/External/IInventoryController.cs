using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Menu.Inventory.External
{
    public interface IInventoryController
    {
        void OpenWindow();
        void CloseWindow();
        bool IsOpen();
        bool AddItem(IModuleItemConfig moduleItemConfig);
        bool AddItem(string id);
    }
}