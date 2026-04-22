using System;

namespace App.Game.Containers.ContainerWindow.Runtime
{
    public interface IContainerWindowController
    {
        void OpenWindow(Container.Runtime.Container container, Action onClosed = null);
        void CloseWindow();
    }
}