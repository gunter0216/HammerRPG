using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.External;

namespace App.Game.Containers.ContainerWindow.External
{
    public class ContainerToInventoryController : IInitSystem
    {
        private readonly ContainerWindowController _containerWindow;
        private readonly InventoryController _inventoryController;

        public ContainerToInventoryController(ContainerWindowController containerWindow, InventoryController inventoryController)
        {
            _containerWindow = containerWindow;
            _inventoryController = inventoryController;
        }

        public void Init()
        {
            _containerWindow.OnWindowOpened += OnWindowOpened;
        }

        private void OnWindowOpened()
        {
            _inventoryController.OpenWindow();
        }
    }
}