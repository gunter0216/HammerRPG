using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;

namespace App.Common.Canvases.External
{
    public class CanvasController : IInitSystem, ICanvasController
    {
        private readonly IAssetManager _assetManager;
        
        private BaseCanvas _hudCanvas;
        private BaseCanvas _windowsCanvas;
        private BaseCanvas _menuCanvas;
        private BaseCanvas _statusBarCanvas;

        public CanvasController(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        public void Init()
        {
            _hudCanvas = CreateCanvas();
            _windowsCanvas = CreateCanvas();
            _menuCanvas = CreateCanvas();
            _statusBarCanvas = CreateStatusBarCanvas();

            _windowsCanvas.Canvas.sortingOrder = 100;
            _windowsCanvas.name = "WindowsCanvas";

            _hudCanvas.Canvas.sortingOrder = 500;
            _hudCanvas.name = "HUDCanvas";

            _menuCanvas.Canvas.sortingOrder = 1000;
            _menuCanvas.name = "MenuCanvas";
        }

        public ICanvas GetHudCanvas()
        {
            return _hudCanvas;
        }
        
        public ICanvas GetWindowCanvas()
        {
            return _windowsCanvas;
        }
        
        public ICanvas GetMenuCanvas()
        {
            return _menuCanvas;
        }
        
        public ICanvas GetStatusBarCanvas()
        {
            return _statusBarCanvas;
        }

        private BaseCanvas CreateCanvas()
        {
            var canvas = _assetManager.InstantiateSync<BaseCanvas>(new StringKeyEvaluator("BaseCanvas"));
            if (!canvas.HasValue)
            {
                HLogger.LogError("Cant load canvas.");
            }
            
            return canvas.Value;
        }
        
        private BaseCanvas CreateStatusBarCanvas()
        {
            var canvas = _assetManager.InstantiateSync<BaseCanvas>(new StringKeyEvaluator("StatusBarCanvas"));
            if (!canvas.HasValue)
            {
                HLogger.LogError("Cant load canvas.");
            }
            
            return canvas.Value;
        }
    }
}