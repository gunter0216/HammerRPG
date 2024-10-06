using System;
using App.Menu.UI.External.View.Panels;

namespace App.Menu.UI.External.FSM.States
{
    public class SingleplayerMenuState : IMenuState, IDisposable
    {
        private readonly MenuFSM m_MenuFsm;
        private readonly SingleplayerPanel m_SingleplayerPanel;

        public SingleplayerMenuState(MenuFSM menuFsm, SingleplayerPanel singleplayerPanel)
        {
            m_SingleplayerPanel = singleplayerPanel;
            m_MenuFsm = menuFsm;
            
            m_SingleplayerPanel.SetActive(false);
            
            m_SingleplayerPanel.SubscribeToBackButtonClick(OnBackButtonClick);
        }
        
        public void Enter()
        {
            m_SingleplayerPanel.SetActive(true);
        }

        public void Exit()
        {
            m_SingleplayerPanel.SetActive(false);
        }
        
        private void OnBackButtonClick()
        {
            m_MenuFsm.PopState();
        }

        public void Dispose()
        {
            m_SingleplayerPanel?.UnSubscribeToBackButtonClick(OnBackButtonClick);
        }
    }
}