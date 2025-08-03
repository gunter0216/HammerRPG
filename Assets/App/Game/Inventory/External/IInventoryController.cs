namespace App.Menu.Inventory.External
{
    public interface IInventoryController
    {
        void OpenWindow();
        void CloseWindow();
        bool IsOpen();
    }
}