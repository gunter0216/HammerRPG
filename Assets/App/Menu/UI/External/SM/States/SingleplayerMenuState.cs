using System;
using App.Menu.UI.External.View.Panels;
using App.Menu.UI.External.View.Panels.Singleplayer;

namespace App.Menu.UI.External.FSM.States
{
    public class SingleplayerMenuState : IMenuState, IDisposable
    {
        private readonly MenuMachine m_MenuMachine;
        private readonly SingleplayerPanel m_SingleplayerPanel;

        public SingleplayerMenuState(MenuMachine menuMachine, SingleplayerPanel singleplayerPanel)
        {
            m_SingleplayerPanel = singleplayerPanel;
            m_MenuMachine = menuMachine;
            
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
            m_MenuMachine.PopState();
        }

        public void Dispose()
        {
            m_SingleplayerPanel?.UnSubscribeToBackButtonClick(OnBackButtonClick);
        }
    }
}