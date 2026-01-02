using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.DragItem.Runtime.View;

namespace App.Game.DragItem.Runtime
{
    public class DragItemViewCreator
    {
        public const string AssetKey = "DragItemView";
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;
        
        public DragItemViewCreator(IAssetManager assetManager, ICanvas canvas)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
        }
        
        public Optional<ItemView> Create()
        {
            var window = m_AssetManager.InstantiateSync<ItemView>(
                new StringKeyEvaluator(AssetKey),
                m_Canvas.GetContent());
            return window;
        }
    }
}