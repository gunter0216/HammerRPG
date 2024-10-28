using System;
using App.Game.Utility.Runtime.MenuSM;

namespace App.Game.Settings.Runtime
{
    public class SettingsMenuState : IMenuState, IDisposable
    {
        private readonly SettingsPanel m_SettingsPanel;
        private readonly MenuMachine m_MenuMachine;

        public SettingsMenuState(MenuMachine menuMachine, SettingsPanel settingsPanel)
        {
            m_SettingsPanel = settingsPanel;
            m_MenuMachine = menuMachine;
            
            m_SettingsPanel.SetActive(false);
            
            m_SettingsPanel.SubscribeToBackButtonClick(OnBackButtonClick);
        }
        
        public void Enter()
        {
            m_SettingsPanel.SetActive(true);
        }

        public void Exit()
        {
            m_SettingsPanel.SetActive(false);
        }
        
        private void OnBackButtonClick()
        {
            m_MenuMachine.PopState();
        }

        public void Dispose()
        {
            m_SettingsPanel?.UnSubscribeToBackButtonClick(OnBackButtonClick);
        }
    }
}