using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Common.Windows.Runtime;

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
            _containerWindow.OnWindowPreOpened += OnWindowOpened;
            _containerWindow.OnWindowClosed += OnWindowClosed;
        }

        private void OnWindowClosed()
        {
            _windowManager.Close(WindowNames.Inventory);
        }

        private void OnWindowOpened()
        {
            _windowManager.Open(WindowNames.Inventory);
        }
    }
}