namespace App.Game.ContainerWindow.Runtime
{
    public interface IContainerWindowController
    {
        void OpenWindow(Container.Runtime.Container container);
        void CloseWindow();
    }
}