using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Common.Windows.Runtime;
using App.Game.DragItem.Runtime;
using App.Game.Inventory.External;

namespace App.Game.Containers.ContainerWindow.External
{
    public class ContainerToInventoryController : IInitSystem
    {
        private readonly ContainerWindowController _containerWindow;
        private readonly IWindowManager _windowManager;
        private readonly InventoryController _inventory;
        private readonly DragItemController _drag;

        public ContainerToInventoryController(
            ContainerWindowController containerWindow, 
            IWindowManager windowManager,
            InventoryController inventory,
            DragItemController drag)
        {
            _containerWindow = containerWindow;
            _windowManager = windowManager;
            _inventory = inventory;
            _drag = drag;
        }

        public void Init()
        {
            _containerWindow.OnWindowOpened += OnWindowOpened;
            _containerWindow.OnWindowClosed += OnWindowClosed;
            _inventory.WindowClosed += OnInventoryClosed;
        }

        private void OnWindowClosed()
        {
            _drag.CancelDrag();
            _windowManager.Close(WindowNames.Inventory);
        }

        private void OnInventoryClosed()
        {
            if (_containerWindow.IsOpen())
                _containerWindow.CloseWindow();
        }

        private void OnWindowOpened()
        {
            _windowManager.Open(WindowNames.Inventory);
        }
    }
}
