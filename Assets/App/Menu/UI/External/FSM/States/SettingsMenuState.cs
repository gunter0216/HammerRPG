using System;
using App.Menu.UI.External.View.Panels;

namespace App.Menu.UI.External.FSM.States
{
    public class SettingsMenuState : IMenuState, IDisposable
    {
        private readonly SettingsPanel m_SettingsPanel;
        private readonly MenuFSM m_MenuFsm;

        public SettingsMenuState(MenuFSM menuFsm, SettingsPanel settingsPanel)
        {
            m_SettingsPanel = settingsPanel;
            m_MenuFsm = menuFsm;
            
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
            m_MenuFsm.PopState();
        }

        public void Dispose()
        {
            m_SettingsPanel?.UnSubscribeToBackButtonClick(OnBackButtonClick);
        }
    }
}