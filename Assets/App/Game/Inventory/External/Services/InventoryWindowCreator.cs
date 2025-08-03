using App.Common.AssetSystem.Runtime;
using App.Common.Utility.Runtime;
using App.Game.Inventory.External.View;

namespace App.Game.Inventory.External.ViewController
{
    public class InventoryWindowCreator
    {
        public const string WindowKey = "InventoryWindow";
        
        private readonly IAssetManager m_AssetManager;

        public InventoryWindowCreator(IAssetManager assetManager)
        {
            m_AssetManager = assetManager;
        }

        public Optional<InventoryWindow> Create()
        {
            var window = m_AssetManager.InstantiateSync<InventoryWindow>(new StringKeyEvaluator(WindowKey));
            return window;
        }
    }
}