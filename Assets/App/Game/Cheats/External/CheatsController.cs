using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.ModuleItem.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.ViewModel;
using App.Game.Contexts;
using App.Game.GameItems.External;
using App.Game.Inventory.External;
using App.Game.SpriteLoaders.Runtime;
using App.Game.States.Runtime.Game;
using Assets.App.Game.GameItems.Runtime;

namespace App.Game.Cheats.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 1000)]
    public class CheatsController : IInitSystem
    {
        [Inject] private readonly IModuleItemsManager m_ModuleItemsManager;
        [Inject] private readonly InventoryController m_InventoryController;
        [Inject] private readonly ISpriteLoader m_SpriteLoader;
        [Inject] private readonly PopupCanvas m_PopupCanvas;
        [Inject] private readonly IAssetManager m_AssetManager;
        [Inject] private readonly IGameItemsManager m_GameItemsManager;
        
        private CheatsWindowModel m_CheatsWindowModel;
        
        public void Init()
        {
            var configs = m_ModuleItemsManager.GetConfigs(GameItemsConstants.ModuleItemType);
            var groups = m_InventoryController.GetGroups();

            m_CheatsWindowModel = new CheatsWindowModel(
                m_AssetManager,
                m_PopupCanvas,
                m_SpriteLoader,
                m_GameItemsManager,
                m_InventoryController,
                configs.Value,
                groups);
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