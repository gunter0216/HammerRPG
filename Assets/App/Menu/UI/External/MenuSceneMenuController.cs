using System;
using App.Common.AssetSystem.Runtime;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using App.Game;
using App.Game.Canvases;
using App.Game.Contexts;
using App.Menu.UI.External.FSM;
using App.Menu.UI.External.FSM.States;
using App.Menu.UI.External.View;

namespace App.Menu.UI.External
{
    [Scoped(typeof(MenuSceneContext))]
    public class MenuSceneMenuController : IInitSystem, IDisposable
    {
        private const string m_MenuSceneMenuAssetKey = "MenuSceneMenuView";
        private readonly StringKeyEvaluator m_MenuSceneMenuAssetKeyEvaluator = new(m_MenuSceneMenuAssetKey);
        
        [Inject] private MainCanvas m_MainCanvas;
        [Inject] private IAssetManager m_AssetManager;

        private MainMenuState m_MainMenuState;
        private MultiplayerMenuState m_MultiplayerMenuState;
        private SettingsMenuState m_SettingsMenuState;
        private SingleplayerMenuState m_SingleplayerMenuState;
        private MenuFSM m_MenuFsm;
        
        private MenuSceneMenuView m_View;

        public void Init()
        {
            var view = m_AssetManager.InstantiateSync<MenuSceneMenuView>(
                m_MenuSceneMenuAssetKeyEvaluator, 
                m_MainCanvas.GetContent());
            if (!view.HasValue)
            {
                HLogger.LogError("cant create MenuSceneMenuView");
                return;
            }

            m_View = view.Value;

            m_MenuFsm = new MenuFSM();

            m_SingleplayerMenuState = new SingleplayerMenuState(m_MenuFsm, m_View.SingleplayerPanel);
            m_MultiplayerMenuState = new MultiplayerMenuState(m_MenuFsm, m_View.MultiplayerPanel);
            m_SettingsMenuState = new SettingsMenuState(m_MenuFsm, m_View.SettingsPanel);
            m_MainMenuState = new MainMenuState(
                m_MenuFsm, 
                m_View.MainMenuPanel, 
                m_SingleplayerMenuState, 
                m_MultiplayerMenuState, 
                m_SettingsMenuState);
            
            m_MenuFsm.PushState(m_MainMenuState);
        }

        public void Dispose()
        {
            m_MultiplayerMenuState?.Dispose();
            m_SettingsMenuState?.Dispose();
            m_SingleplayerMenuState?.Dispose();
            m_MainMenuState?.Dispose();
        }
    }
}