using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Inventory.External
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