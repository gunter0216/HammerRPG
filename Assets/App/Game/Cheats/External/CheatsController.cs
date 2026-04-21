using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.ViewModel;
using App.Game.Inventory.External;

namespace App.Game.Cheats.External
{
    public class CheatsController : IInitSystem
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly InventoryController _inventoryController;
        private readonly ISpriteLoader _spriteLoader;
        private readonly ICanvasController _canvasController;
        private readonly IAssetManager _assetManager;

        private CheatsWindowModel _cheatsWindowModel;

        public CheatsController(
            IModuleItemsManager moduleItemsManager, 
            InventoryController inventoryController,
            ISpriteLoader spriteLoader,
            ICanvasController canvasController,
            IAssetManager assetManager)
        {
            _moduleItemsManager = moduleItemsManager;
            _inventoryController = inventoryController;
            _spriteLoader = spriteLoader;
            _canvasController = canvasController;
            _assetManager = assetManager;
        }

        public void Init()
        {
            var configs = _moduleItemsManager.GetConfigs(ModuleItemConfigs.GameItemsType);

            _cheatsWindowModel = new CheatsWindowModel(
                _assetManager,
                _canvasController,
                _spriteLoader,
                _inventoryController,
                configs.Value);
        }

        public bool IsOpen()
        {
            return _cheatsWindowModel.IsOpen();
        }

        public void CloseWindow()
        {
            _cheatsWindowModel.Close();
        }

        public void OpenWindow()
        {
            _cheatsWindowModel.Open();
        }
    }
}