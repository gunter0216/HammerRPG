using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SceneControllers.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.External;
using App.Common.Windows.Runtime;
using App.Game.Canvases.External;
using App.Game.GameMenu.Runtime;
using App.Game.GameMenu.Runtime.View;
using App.Game.Pause.Runtime;
using App.Game.Settings.Runtime;
using App.Game.Utility.Runtime.MenuSM;
using UnityEngine;

namespace App.Game.GameMenu.External
{
    public class GameMenuController : IInitSystem, IRunSystem, IDisposable, IWindowController
    {
        private const string m_GameMenuAssetKey = "GameMenuView";
        private readonly StringKeyEvaluator m_GameMenuAssetKeyEvaluator = new(m_GameMenuAssetKey);

        private readonly IWindowManager _windowManager;
        private readonly MainCanvas _mainCanvas;
        private readonly IAssetManager _assetManager;
        private readonly IDataManager _dataManager;
        private readonly ISceneManager _sceneManager;
        private readonly IPauseController _pauseController;
        
        private GameMenuState m_GameMenuState;
        private SettingsMenuState m_SettingsMenuState;
        
        private MenuMachine _menuMachine;
        
        private GameMenuView m_View;

        public GameMenuController(
            MainCanvas mainCanvas, 
            IAssetManager assetManager,
            IDataManager dataManager,
            ISceneManager sceneManager,
            IPauseController pauseController, 
            IWindowManager windowManager)
        {
            _mainCanvas = mainCanvas;
            _assetManager = assetManager;
            _dataManager = dataManager;
            _sceneManager = sceneManager;
            _pauseController = pauseController;
            _windowManager = windowManager;
        }

        public void Init()
        {
            var view = _assetManager.InstantiateSync<GameMenuView>(
                m_GameMenuAssetKeyEvaluator,
                _mainCanvas.GetContent());
            if (!view.HasValue)
            {
                HLogger.LogError("cant create GameSceneMenuView");
                return;
            }

            m_View = view.Value;
            m_View.SetActive(false);

            _windowManager.Registry(this, new WindowConfig(
                closeOnEscape: false,
                onClosed: OnClosed,
                onOpened: OnOpened));
            
            _menuMachine = new MenuMachine(popAction: OnPop);
            
            m_SettingsMenuState = new SettingsMenuState(_menuMachine, m_View.SettingsPanel);
            m_GameMenuState = new GameMenuState(
                _menuMachine, 
                m_View.MainMenuPanel, 
                m_SettingsMenuState,
                new SaveAndExitStrategy(_sceneManager, _pauseController));
        }

        private void OnPop(IMenuState _)
        {
            if (_menuMachine.GetCountInStack() <= 0)
            {
                _windowManager.Close(this);
            }
        }

        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_menuMachine.GetCountInStack() <= 0)
                {
                    if (_windowManager.IsAnyOpen())
                    {
                        return;
                    }
                    
                    _windowManager.Open(this);
                }
                else
                {
                    _menuMachine.PopState();
                }
            }
        }

        private void OnOpened()
        {
            _menuMachine.PushState(m_GameMenuState);
            _pauseController.Pause();
        }

        private void OnClosed()
        {
            _pauseController.Unpause();
        }

        public void SetActive(bool status)
        {
            m_View.SetActive(status);
        }

        public WindowNames GetName()
        {
            return WindowNames.MainMenu;
        }

        public void Dispose()
        {
            m_SettingsMenuState?.Dispose();
            m_GameMenuState?.Dispose();
        }
    }
}