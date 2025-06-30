using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.SceneControllers.Runtime;
using App.Game.Canvases.External;
using App.Game.Contexts;
using App.Game.GameMenu.Runtime;
using App.Game.GameMenu.Runtime.View;
using App.Game.Pause.Runtime;
using App.Game.Settings.Runtime;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using App.Game.Utility.Runtime.MenuSM;
using UnityEngine;

namespace App.Game.GameMenu.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    [RunSystem(0)]
    public class GameMenuController : IInitSystem, IRunSystem, IDisposable
    {
        private const string m_GameMenuAssetKey = "GameMenuView";
        private readonly StringKeyEvaluator m_GameMenuAssetKeyEvaluator = new(m_GameMenuAssetKey);
        
        [Inject] private MainCanvas m_MainCanvas;
        [Inject] private IAssetManager m_AssetManager;
        [Inject] private IDataManager m_DataManager;
        [Inject] private ISceneManager m_SceneManager;
        [Inject] private IPauseController m_PauseController;
        
        private GameMenuState m_GameMenuState;
        private SettingsMenuState m_SettingsMenuState;
        
        private MenuMachine m_MenuMachine;
        
        private GameMenuView m_View;

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