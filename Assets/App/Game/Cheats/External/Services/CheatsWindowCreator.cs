using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.View;

namespace App.Game.Cheats.External.Services
{
    public class CheatsWindowCreator : ICheatsWindowCreator
    {
        public const string WindowKey = "CheatsWindow";
        
        private readonly IAssetManager _assetManager;
        private readonly ICanvasController _canvasController;

        public CheatsWindowCreator(IAssetManager assetManager, ICanvasController canvasController)
        {
            _assetManager = assetManager;
            _canvasController = canvasController;
        }

        public Optional<CheatsWindow> Create()
        {
            var window = _assetManager.InstantiateSync<CheatsWindow>(
                new StringKeyEvaluator(WindowKey),
                _canvasController.GetMenuCanvas().GetContent());
            return window;
        }
    }
}