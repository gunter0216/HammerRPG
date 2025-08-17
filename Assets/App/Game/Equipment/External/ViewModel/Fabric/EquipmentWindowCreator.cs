using App.Common.AssetSystem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Equipment.External.View;

namespace App.Game.Equipment.External.ViewModel.Fabric
{
    public class EquipmentWindowCreator
    {
        public const string WindowKey = "EquipmentWindow";
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvas m_Canvas;

        public EquipmentWindowCreator(IAssetManager assetManager, ICanvas canvas)
        {
            m_AssetManager = assetManager;
            m_Canvas = canvas;
        }

        public Optional<EquipmentWindow> Create()
        {
            var window = m_AssetManager.InstantiateSync<EquipmentWindow>(
                new StringKeyEvaluator(WindowKey),
                m_Canvas.GetContent());
            return window;
        }
    }
}
