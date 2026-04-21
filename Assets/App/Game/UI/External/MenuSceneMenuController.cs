using System;
using System.Linq;
using App.Common.AssetSystem.Runtime;
using App.Common.Canvases.External;
using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SceneControllers.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.Settings.Runtime;
using App.Game.UI.External.Data;
using App.Game.UI.External.States;
using App.Game.UI.External.View;
using App.Game.UI.Runtime;
using App.Game.UI.Runtime.Data;
using App.Game.Utility.Runtime.MenuSM;

namespace App.Game.UI.External
{
    public class MenuSceneMenuController : IInitSystem, IDisposable
    {
        private const string m_MenuSceneMenuAssetKey = "MenuSceneMenuView";
        private readonly StringKeyEvaluator m_MenuSceneMenuAssetKeyEvaluator = new(m_MenuSceneMenuAssetKey);
        
        private readonly ICanvasController _canvasController;
        private readonly IAssetManager m_AssetManager;
        private readonly IDataManager m_DataManager;
        private readonly ISceneManager m_SceneManager;

        private CreateGameMenuState m_CreateGameMenuState;
        private MainMenuState m_MainMenuState;
        private MultiplayerMenuState m_MultiplayerMenuState;
        private SettingsMenuState m_SettingsMenuState;
        private SingleplayerMenuState m_SingleplayerMenuState;
        
        private MenuMachine m_MenuMachine;

        private GameRecordsDataController m_DataController;
        private MenuSceneMenuView m_View;

        public MenuSceneMenuController(
            ICanvasController canvasController,
            IAssetManager assetManager, 
            IDataManager dataManager,
            ISceneManager sceneManager)
        {
            _canvasController = canvasController;
            m_AssetManager = assetManager;
            m_DataManager = dataManager;
            m_SceneManager = sceneManager;
        }

        public void Init()
        {
            var view = m_AssetManager.InstantiateSync<MenuSceneMenuView>(
                m_MenuSceneMenuAssetKeyEvaluator,
                _canvasController.GetMenuCanvas().GetContent());
            if (!view.HasValue)
            {
                HLogger.LogError("cant create MenuSceneMenuView");
                return;
            }

            var dataLoader = new GameRecordsDataLoader(m_DataManager);
            m_DataController = new GameRecordsDataController(dataLoader);

            m_View = view.Value;

            m_MenuMachine = new MenuMachine();
            var gameRecordCreateStrategy = new GameRecordCreateStrategy(m_DataController);
            var startGameStrategy = new StartGameStrategy(m_SceneManager, m_DataController);
            
            m_CreateGameMenuState = new CreateGameMenuState(m_MenuMachine, m_View.CreateGamePanel, gameRecordCreateStrategy);
            m_SingleplayerMenuState = new SingleplayerMenuState(
                m_MenuMachine, 
                m_View.SingleplayerPanel,
                m_CreateGameMenuState,
                m_DataController,
                startGameStrategy);
            m_MultiplayerMenuState = new MultiplayerMenuState(m_MenuMachine, m_View.MultiplayerPanel);
            m_SettingsMenuState = new SettingsMenuState(m_MenuMachine, m_View.SettingsPanel);
            m_MainMenuState = new MainMenuState(
                m_MenuMachine, 
                m_View.MainMenuPanel, 
                m_SingleplayerMenuState, 
                m_MultiplayerMenuState, 
                m_SettingsMenuState);
            
            m_MenuMachine.PushState(m_MainMenuState);

            var record = m_DataController.GetRecords().FirstOrDefault();
            if (record != null)
            {
                startGameStrategy.StartGame(record.Name);
            }
        }

        public void Dispose()
        {
            m_CreateGameMenuState?.Dispose();
            m_MultiplayerMenuState?.Dispose();
            m_SettingsMenuState?.Dispose();
            m_SingleplayerMenuState?.Dispose();
            m_MainMenuState?.Dispose();
        }
    }
}