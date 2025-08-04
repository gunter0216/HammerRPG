using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.ModuleItem.Runtime;
using App.Game.Canvases.External;
using App.Game.Cheats.External.ViewModel;
using App.Game.Contexts;
using App.Game.GameItems.External;
using App.Game.SpriteLoaders.Runtime;
using App.Game.States.Game;
using App.Menu.Inventory.External;

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
        
        private CheatsWindowModel m_CheatsWindowModel;
        
        public void Init()
        {
            var configs = m_ModuleItemsManager.GetConfigs(GameItemsConstants.ModuleItemType);
            var groups = m_InventoryController.GetGroups();

            m_CheatsWindowModel = new CheatsWindowModel(
                m_AssetManager,
                m_PopupCanvas,
                m_SpriteLoader,
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