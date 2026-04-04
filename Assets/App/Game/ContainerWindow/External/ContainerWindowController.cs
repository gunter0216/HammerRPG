using System;
using App.Common.AssetSystem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.ContainerWindow.External.ViewModel;
using App.Game.ContainerWindow.Runtime;

namespace App.Game.ContainerWindow.External
{
    public class ContainerWindowController : IInitSystem, IContainerWindowController
    {
        private readonly IAssetManager m_AssetManager;
        private readonly PopupCanvas m_Canvas;
        private readonly ISpriteLoader m_SpriteLoader;

        private ContainerWindowModel m_WindowModel;

        public Action OnWindowOpened;

        public ContainerWindowController(IAssetManager assetManager, PopupCanvas canvas, ISpriteLoader spriteLoader)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
            m_SpriteLoader = spriteLoader;
        }

        public void Init()
        {
            m_WindowModel = new ContainerWindowModel(m_AssetManager, m_Canvas, m_SpriteLoader);
        }

        public void OpenWindow(Container.Runtime.Container container)
        {
            m_WindowModel.Open(container);
            OnWindowOpened?.Invoke();
        }

        public void CloseWindow()
        {
            m_WindowModel.Close();
        }
    }
}