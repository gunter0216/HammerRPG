using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameMenu.External.View;

namespace App.Game.GameMenu.External.Fabric
{
    public class HUDCreator
    {
        private const string _assetKey = "HUDView";
        
        private readonly IAssetManager _assetManager;
        private readonly ICanvasController _canvasController;

        public HUDCreator(IAssetManager assetManager, ICanvasController canvasController)
        {
            _assetManager = assetManager;
            _canvasController = canvasController;
        }

        public Optional<HUDView> Create()
        {
            var view = _assetManager.InstantiateSync<HUDView>(
                new StringKeyEvaluator(_assetKey),
                _canvasController.GetHudCanvas().GetContent());
            if (!view.HasValue)
            {
                HLogger.LogError("Cant create hud.");
                return Optional<HUDView>.Fail();
            }
            
            return Optional<HUDView>.Success(view.Value);
        }
    }
}