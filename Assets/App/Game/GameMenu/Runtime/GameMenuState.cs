using System;
using App.Common.SceneControllers.Runtime;
using App.Game.GameMenu.Runtime.View;
using App.Game.Settings.Runtime;
using App.Game.Utility.Runtime.MenuSM;

namespace App.Game.GameMenu.Runtime
{
    public class GameMenuState : IMenuState, IDisposable
    {
        private readonly SettingsMenuState m_SettingsMenuState;
        private readonly GameMenuPanel m_GameMenuPanel;
        private readonly MenuMachine m_MenuMachine;
        private readonly ISaveGameStrategy m_SaveGameStrategy;

        public GameMenuState(
            MenuMachine menuMachine, 
            GameMenuPanel gameMenuPanel, 
            SettingsMenuState settingsMenuState, 
            ISaveGameStrategy saveGameStrategy)
        {
            m_GameMenuPanel = gameMenuPanel;
            m_SettingsMenuState = settingsMenuState;
            m_SaveGameStrategy = saveGameStrategy;
            m_MenuMachine = menuMachine;
            
            m_GameMenuPanel.SetActive(false);
            
            m_GameMenuPanel.SetContinueButtonClickAction(OnContinueButtonClick);
            m_GameMenuPanel.SetSettingsButtonClickAction(OnSettingsButtonClick);
            m_GameMenuPanel.SetExitButtonClickAction(OnExitButtonClick);
        }

        public void Enter()
        {
            m_GameMenuPanel.SetActive(true);
        }

        public void Exit()
        {
            m_GameMenuPanel.SetActive(false);
        }

        private void OnContinueButtonClick()
        {
            m_MenuMachine.PopState();
        }

        private void OnExitButtonClick()
        {
            m_SaveGameStrategy.Save();
        }

        private void OnSettingsButtonClick()
        {
            m_MenuMachine.PushState(m_SettingsMenuState);
        }

        public void Dispose()
        {

        }
    }
}