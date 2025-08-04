using App.Common.AssetSystem.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.View;
using App.Common.Utility.Runtime;

namespace App.Game.Cheats.External.Services
{
    public class CheatsWindowCreator : ICheatsWindowCreator
    {
        public const string WindowKey = "CheatsWindow";
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;

        public CheatsWindowCreator(IAssetManager assetManager, ICanvas canvas)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
        }

        public Optional<CheatsWindow> Create()
        {
            var window = m_AssetManager.InstantiateSync<CheatsWindow>(
                new StringKeyEvaluator(WindowKey),
                m_Canvas.GetContent());
            return window;
        }
    }
}