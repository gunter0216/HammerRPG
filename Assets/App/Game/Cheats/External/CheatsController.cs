using App.Common.AssetSystem.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.ViewModel;
using App.Game.GameItems.Runtime;
using App.Game.Inventory.External;

namespace App.Game.Cheats.External
{
    public class CheatsController : IInitSystem
    {
        private readonly IModuleItemsManager m_ModuleItemsManager;
        private readonly InventoryController m_InventoryController;
        private readonly ISpriteLoader m_SpriteLoader;
        private readonly PopupCanvas m_PopupCanvas;
        private readonly IAssetManager m_AssetManager;
        private readonly IGameItemsManager m_GameItemsManager;

        private CheatsWindowModel m_CheatsWindowModel;

        public CheatsController(
            IModuleItemsManager moduleItemsManager, 
            InventoryController inventoryController,
            ISpriteLoader spriteLoader,
            PopupCanvas popupCanvas,
            IAssetManager assetManager,
            IGameItemsManager gameItemsManager)
        {
            m_ModuleItemsManager = moduleItemsManager;
            m_InventoryController = inventoryController;
            m_SpriteLoader = spriteLoader;
            m_PopupCanvas = popupCanvas;
            m_AssetManager = assetManager;
            m_GameItemsManager = gameItemsManager;
        }

        public void Init()
        {
            var configs = m_ModuleItemsManager.GetConfigs(GameItemsConstants.ModuleItemType);

            m_CheatsWindowModel = new CheatsWindowModel(
                m_AssetManager,
                m_PopupCanvas,
                m_SpriteLoader,
                m_GameItemsManager,
                m_InventoryController,
                configs.Value);
        }

        public bool IsOpen()
        {
            return m_CheatsWindowModel.IsOpen();
        }

        public void CloseWindow()
        {
            m_CheatsWindowModel.Close();
        }

        public void OpenWindow()
        {
            m_CheatsWindowModel.Open();
        }
    }
}