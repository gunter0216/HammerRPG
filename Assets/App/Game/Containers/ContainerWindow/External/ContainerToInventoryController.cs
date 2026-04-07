using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Common.Windows.Runtime;
using App.Game.Inventory.External;

namespace App.Game.Containers.ContainerWindow.External
{
    public class ContainerToInventoryController : IInitSystem
    {
        private readonly ContainerWindowController _containerWindow;
        private readonly IWindowManager _windowManager;

        public ContainerToInventoryController(
            ContainerWindowController containerWindow, 
            IWindowManager windowManager)
        {
            _containerWindow = containerWindow;
            _windowManager = windowManager;
        }

        public void Init()
        {
            _containerWindow.OnWindowOpened += OnWindowOpened;
        }

        private void OnWindowOpened()
        {
            _windowManager.Open(WindowNames.Inventory);
        }
    }
}