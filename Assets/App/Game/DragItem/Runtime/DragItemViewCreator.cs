using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.DragItem.Runtime.View;

namespace App.Game.DragItem.Runtime
{
    public class DragItemViewCreator
    {
        public const string AssetKey = "DragItemView";
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvasController _canvasController;
        
        public DragItemViewCreator(IAssetManager assetManager, ICanvasController canvasController)
        {
            m_AssetManager = assetManager;
            _canvasController = canvasController;
        }
        
        public Optional<ItemView> Create()
        {
            var window = m_AssetManager.InstantiateSync<ItemView>(
                new StringKeyEvaluator(AssetKey),
                _canvasController.GetWindowCanvas().GetContent());
            return window;
        }
    }
}