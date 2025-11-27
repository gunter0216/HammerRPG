using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SceneControllers.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.GameMenu.Runtime;
using App.Game.GameMenu.Runtime.View;
using App.Game.Pause.Runtime;
using App.Game.Settings.Runtime;
using App.Game.Utility.Runtime.MenuSM;
using UnityEngine;

namespace App.Game.GameMenu.External
{
    public class GameMenuController : IInitSystem, IRunSystem, IDisposable
    {
        private const string m_GameMenuAssetKey = "GameMenuView";
        private readonly StringKeyEvaluator m_GameMenuAssetKeyEvaluator = new(m_GameMenuAssetKey);
        
        private readonly MainCanvas m_MainCanvas;
        private readonly IAssetManager m_AssetManager;
        private readonly IDataManager m_DataManager;
        private readonly ISceneManager m_SceneManager;
        private readonly IPauseController m_PauseController;
        
        private GameMenuState m_GameMenuState;
        private SettingsMenuState m_SettingsMenuState;
        
        private MenuMachine m_MenuMachine;
        
        private GameMenuView m_View;

        public GameMenuController(
            MainCanvas mainCanvas, 
            IAssetManager assetManager,
            IDataManager dataManager,
            ISceneManager sceneManager,
            IPauseController pauseController)
        {
            m_MainCanvas = mainCanvas;
            m_AssetManager = assetManager;
            m_DataManager = dataManager;
            m_SceneManager = sceneManager;
            m_PauseController = pauseController;
        }

        public void Init()
        {
            var view = m_AssetManager.InstantiateSync<GameMenuView>(
                m_GameMenuAssetKeyEvaluator,
                m_MainCanvas.GetContent());
            if (!view.HasValue)
            {
                HLogger.LogError("cant create GameSceneMenuView");
                return;
            }

            m_View = view.Value;
            m_View.SetActive(false);

            m_MenuMachine = new MenuMachine(popAction: OnPop);
            
            m_SettingsMenuState = new SettingsMenuState(m_MenuMachine, m_View.SettingsPanel);
            m_GameMenuState = new GameMenuState(
                m_MenuMachine, 
                m_View.MainMenuPanel, 
                m_SettingsMenuState,
                new SaveAndExitStrategy(m_SceneManager, m_PauseController));
        }

        private void OnPop(IMenuState _)
        {
            if (m_MenuMachine.GetCountInStack() <= 0)
            {
                m_View.SetActive(false);
                m_PauseController.Unpause();
            }
        }

        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_MenuMachine.GetCountInStack() <= 0)
                {
                    m_View.SetActive(true);
                    m_MenuMachine.PushState(m_GameMenuState);
                    m_PauseController.Pause();
                }
            }
        }

        public void Dispose()
        {
            m_SettingsMenuState?.Dispose();
            m_GameMenuState?.Dispose();
        }
    }
}