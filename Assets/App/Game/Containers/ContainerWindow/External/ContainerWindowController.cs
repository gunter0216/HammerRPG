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

        public Action OnWindowOpened;
        
        private ContainerSlotsModel _slotsModel;
        private Container.Runtime.Container _container;

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
        }

        public void OpenWindow(Container.Runtime.Container container)
        {
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

            _slotsModel.ShowContainer(_container);
            
            OnWindowOpened?.Invoke();
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