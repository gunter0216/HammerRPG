using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Equipment.External.View;

namespace App.Game.Equipment.External.ViewModel.Fabric
{
    public class EquipmentWindowCreator
    {
        public const string WindowKey = "EquipmentWindow";
        
        private readonly IAssetManager m_AssetManager;
        private readonly ICanvasController _canvasController;

        public EquipmentWindowCreator(IAssetManager assetManager, ICanvasController canvasController)
        {
            m_AssetManager = assetManager;
            _canvasController = canvasController;
        }

        public Optional<EquipmentWindow> Create()
        {
            var window = m_AssetManager.InstantiateSync<EquipmentWindow>(
                new StringKeyEvaluator(WindowKey),
                _canvasController.GetWindowCanvas().GetContent());
            return window;
        }
    }
}
