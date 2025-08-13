using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Inventory.External.View;

namespace App.Game.Inventory.External.Services
{
    public class InventoryWindowCreator
    {
        public const string WindowKey = "InventoryWindow";
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;

        public InventoryWindowCreator(IAssetManager assetManager, ICanvas canvas)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
        }

        public Optional<InventoryWindow> Create()
        {
            var window = m_AssetManager.InstantiateSync<InventoryWindow>(
                new StringKeyEvaluator(WindowKey),
                m_Canvas.GetContent());
            return window;
        }
    }
}