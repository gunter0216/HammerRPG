using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Common.Windows.Runtime;
using App.Game.Containers.ContainerWindow.External.ViewModel.Fabric;
using App.Game.Containers.ContainerWindow.External.ViewModel.Slots;
using App.Game.Containers.ContainerWindow.Runtime;

namespace App.Game.Containers.ContainerWindow.External
{
    public class ContainerWindowController : BaseWindowController<View.ContainerWindow>, IInitSystem, IContainerWindowController
    {
        public const string WindowKey = "ContainerWindow";
        
        private readonly ISpriteLoader _spriteLoader;
        
        private ContainerSlotsModel _slotsModel;
        private Container.Runtime.Container _container;
        private Action _onClosedTemp;

        private Action _onWindowPreOpened;
        private Action _onWindowOpened;
        private Action _onWindowClosed;
        
        public Action OnWindowOpened
        {
            get => _onWindowOpened;
            set => _onWindowOpened = value;
        }

        public Action OnWindowClosed
        {
            get => _onWindowClosed;
            set => _onWindowClosed = value;
        }

        public Action OnWindowPreOpened
        {
            get => _onWindowPreOpened;
            set => _onWindowPreOpened = value;
        }

        public ContainerWindowController(
            IAssetManager assetManager,
            ICanvasController canvasController,
            ISpriteLoader spriteLoader,
            IWindowManager windowManager) : base(windowManager, assetManager, canvasController)
        {
            _spriteLoader = spriteLoader;
        }

        public void Init()
        {
        }

        protected override void OnInitWindow()
        {
            base.OnInitWindow();
            
            _slotsModel = new ContainerSlotsModel(
                new ContainerSlotViewCreator(_window),
                _spriteLoader);
            _slotsModel.Initialize();
            _window.SetCloseButtonClickCallback(OnCloseClick);
        }

        private void OnCloseClick()
        {
            Close();
        }

        public void OpenWindow(Container.Runtime.Container container, Action onClosed = null)
        {
            if (container == null || IsOpen())
                return;

            _onWindowPreOpened?.Invoke();
            
            _onClosedTemp = onClosed;
            _container = container;
            Open();
        }

        public void CloseWindow()
        {
            Close();
        }

        protected override void OnOpened()
        {
            base.OnOpened();

            _window.SetColumns(_container.Columns);
            _slotsModel.ShowContainer(_container);
            
            _onWindowOpened?.Invoke();
            // The inventory backdrop spans the screen and must sit below the container.
            _window.transform.SetAsLastSibling();
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            _slotsModel.HideContainer();
            _container = null;

            var onClosed = _onClosedTemp;
            _onClosedTemp = null;
            onClosed?.Invoke();
            _onWindowClosed?.Invoke();
        }

        protected override string GetWindowAssetKey()
        {
            return WindowKey;
        }

        public override WindowNames GetName()
        {
            return WindowNames.Container;
        }
    }
}
