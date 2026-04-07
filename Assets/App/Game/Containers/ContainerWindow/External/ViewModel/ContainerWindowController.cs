using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Windows.External;
using App.Common.Windows.Runtime;
using App.Game.Canvases.External;
using App.Game.Containers.ContainerWindow.External.ViewModel.Fabric;
using App.Game.Containers.ContainerWindow.External.ViewModel.Slots;

namespace App.Game.Containers.ContainerWindow.External.ViewModel
{
    public class ContainerWindowController : IWindowController, IDisposable
    {
        private readonly IAssetManager _assetManager;
        private readonly ICanvas _canvas;
        private readonly ISpriteLoader _spriteLoader;
        private readonly IWindowManager _windowManager;

        private View.ContainerWindow _window;
        private ContainerSlotsModel _slotsModel;

        public ContainerWindowController(
            IAssetManager assetManager,
            ICanvas canvas,
            ISpriteLoader spriteLoader, 
            IWindowManager windowManager)
        {
            _assetManager = assetManager;
            _canvas = canvas;
            _spriteLoader = spriteLoader;
            _windowManager = windowManager;
        }

        public void Open(Container.Runtime.Container container)
        {
            if (_window == null)
            {
                if (!CreateWindow())
                {
                    HLogger.LogError("Failed to create Container window.");
                    return;
                }
            }

            _windowManager.Open(GetName());
            ShowContainer(container);
        }

        private void ShowContainer(Container.Runtime.Container container)
        {
            _slotsModel.ShowContainer(container);
        }

        public void Close()
        {
            _windowManager.Close(GetName());
        }
        
        public bool IsOpen()
        {
            return _window != null && _window.IsActive();
        }
        
        private bool CreateWindow()
        {
            var windowCreator = new ContainerWindowCreator(_assetManager, _canvas);
            var window = windowCreator.Create();
            if (!window.HasValue)
            {
                return false;
            }

            _window = window.Value;
            InitWindow();
            
            return true;
        }

        private void InitWindow()
        {
            _windowManager.Registry(this, new WindowConfig());
            InitSlots();
            
            _window.SetCloseButtonClickCallback(OnCloseButtonClick);
        }
        
        private void OnCloseButtonClick()
        {
            Close();
        }

        private void InitSlots()
        {
            _slotsModel = new ContainerSlotsModel(
                new ContainerSlotViewCreator(_window),
                _spriteLoader);
            _slotsModel.Initialize();
        }

        public void Dispose()
        {
            _slotsModel?.Dispose();
        }

        public void SetActive(bool status)
        {
            _window.SetActive(status);
        }

        public WindowNames GetName()
        {
            return WindowNames.Container;
        }
    }
}