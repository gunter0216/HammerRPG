using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;

namespace App.Game.Containers.ContainerWindow.External.ViewModel.Fabric
{
    public class ContainerWindowCreator
    {
        public const string WindowKey = "ContainerWindow";
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;

        public ContainerWindowCreator(IAssetManager assetManager, ICanvas canvas)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
        }

        public Optional<View.ContainerWindow> Create()
        {
            var window = m_AssetManager.InstantiateSync<View.ContainerWindow>(
                new StringKeyEvaluator(WindowKey),
                m_Canvas.GetContent());
            return window;
        }
    }
}