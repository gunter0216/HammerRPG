using System;
using App.Common.AssetSystem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Game.Canvases.External;
using App.Game.Containers.ContainerWindow.External.ViewModel;
using App.Game.Containers.ContainerWindow.Runtime;

namespace App.Game.Containers.ContainerWindow.External
{
    public class ContainerWindowController : IInitSystem, IContainerWindowController
    {
        private readonly IWindowManager _windowManager;
        private readonly IAssetManager _assetManager;
        private readonly PopupCanvas _canvas;
        private readonly ISpriteLoader _spriteLoader;

        private ViewModel.ContainerWindowController _windowController;

        public Action OnWindowOpened;

        public ContainerWindowController(IAssetManager assetManager, PopupCanvas canvas, ISpriteLoader spriteLoader, IWindowManager windowManager)
        {
            _assetManager = assetManager;
            _canvas = canvas;
            _spriteLoader = spriteLoader;
            _windowManager = windowManager;
        }

        public void Init()
        {
            _windowController = new ViewModel.ContainerWindowController(
                _assetManager, 
                _canvas, 
                _spriteLoader,
                _windowManager);
        }

        public void OpenWindow(Container.Runtime.Container container)
        {
            _windowController.Open(container);
            OnWindowOpened?.Invoke();
        }

        public void CloseWindow()
        {
            _windowController.Close();
        }
    }
}