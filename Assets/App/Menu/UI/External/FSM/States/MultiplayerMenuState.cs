using System;
using App.Menu.UI.External.View.Panels;

namespace App.Menu.UI.External.FSM.States
{
    public class MultiplayerMenuState : IMenuState, IDisposable
    {
        private readonly MultiplayerPanel m_MultiplayerPanel;
        private readonly MenuFSM m_MenuFsm;

        public MultiplayerMenuState(MenuFSM menuFsm, MultiplayerPanel multiplayerPanel)
        {
            m_MultiplayerPanel = multiplayerPanel;
            m_MenuFsm = menuFsm;
            
            m_MultiplayerPanel.SetActive(false);
            
            m_MultiplayerPanel.SubscribeToBackButtonClick(OnBackButtonClick);
        }

        public void Enter()
        {
            m_MultiplayerPanel.SetActive(true);
        }

        public void Exit()
        {
            m_MultiplayerPanel.SetActive(false);
        }

        private void OnBackButtonClick()
        {
            m_MenuFsm.PopState();
        }

        public void Dispose()
        {
            m_MultiplayerPanel?.UnSubscribeToBackButtonClick(OnBackButtonClick);
        }
    }
}