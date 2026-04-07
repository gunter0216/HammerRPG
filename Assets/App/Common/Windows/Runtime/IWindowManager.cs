using App.Common.Windows.Runtime;

namespace App.Common.Windows.External
{
    public interface IWindowManager
    {
        bool IsAnyOpen();
        bool Registry(IWindowController windowController, WindowConfig config);
        bool TryOpen(WindowNames windowName, out IWindowController windowController);
        bool TryClose(WindowNames windowName, out IWindowController windowController);
        bool Open(IWindowController window);
        bool Close(IWindowController window);
        bool Open(WindowNames windowName);
        bool Close(WindowNames windowName);
    }
}