using System;
using App.Menu.UI.External.View.Panels;
using UnityEngine;

namespace App.Menu.UI.External.FSM.States
{
    public class MainMenuState : IMenuState, IDisposable
    {
        private readonly SingleplayerMenuState m_SingleplayerMenuState;
        private readonly MultiplayerMenuState m_MultiplayerMenuState;
        private readonly SettingsMenuState m_SettingsMenuState;
        private readonly MainMenuPanel m_MainMenuPanel;
        private readonly MenuFSM m_MenuFsm;

        public MainMenuState(MenuFSM menuFsm, MainMenuPanel mainMenuPanel, SingleplayerMenuState singleplayerMenuState, MultiplayerMenuState multiplayerMenuState, SettingsMenuState settingsMenuState)
        {
            m_MainMenuPanel = mainMenuPanel;
            m_SingleplayerMenuState = singleplayerMenuState;
            m_MultiplayerMenuState = multiplayerMenuState;
            m_SettingsMenuState = settingsMenuState;
            m_MenuFsm = menuFsm;
            
            m_MainMenuPanel.SetActive(false);
            
            m_MainMenuPanel.SubscribeToSingleplayerButtonClick(OnSingleplayerButtonClick);
            m_MainMenuPanel.SubscribeToMultiplayerButtonClick(OnMultiplayerButtonClick);
            m_MainMenuPanel.SubscribeToSettingsButtonClick(OnSettingsButtonClick);
            m_MainMenuPanel.SubscribeToExitButtonClick(OnExitButtonClick);
        }

        public void Enter()
        {
            m_MainMenuPanel.SetActive(true);
        }

        public void Exit()
        {
            m_MainMenuPanel.SetActive(false);
        }
        
        private void OnExitButtonClick()
        {
            // todo
        }

        private void OnSettingsButtonClick()
        {
            m_MenuFsm.PushState(m_SettingsMenuState);
        }

        private void OnMultiplayerButtonClick()
        {
            m_MenuFsm.PushState(m_MultiplayerMenuState);
        }

        private void OnSingleplayerButtonClick()
        {
            m_MenuFsm.PushState(m_SingleplayerMenuState);
        }

        public void Dispose()
        {
            if (m_MainMenuPanel != null)
            {
                m_MainMenuPanel.UnSubscribeToSingleplayerButtonClick(OnSingleplayerButtonClick);
                m_MainMenuPanel.UnSubscribeToMultiplayerButtonClick(OnMultiplayerButtonClick);
                m_MainMenuPanel.UnSubscribeToSettingsButtonClick(OnSettingsButtonClick);
                m_MainMenuPanel.UnSubscribeToExitButtonClick(OnExitButtonClick);
            }
        }
    }
}