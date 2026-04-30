using System;
using App.Common.Logger.Runtime;
using App.Game.UI.External.View.Panels;
using App.Game.UI.Runtime;
using App.Game.Utility.Runtime.MenuSM;

namespace App.Game.UI.External.States
{
    public class CreateGameMenuState : IMenuState, IDisposable
    {
        private readonly GameRecordCreateStrategy m_RecordCreateStrategy;
        private readonly CreateGamePanel m_Panel;
        private readonly MenuMachine m_MenuMachine;

        public CreateGameMenuState(MenuMachine menuMachine, CreateGamePanel panel, GameRecordCreateStrategy recordCreateStrategy)
        {
            m_MenuMachine = menuMachine;
            m_Panel = panel;
            m_RecordCreateStrategy = recordCreateStrategy;

            m_Panel.SetActive(false);
            
            m_Panel.SetBackButtonAction(OnBackButtonClick);
            m_Panel.SetCreateButtonAction(OnCreateButtonClick);
        }

        public void Enter()
        {
            m_Panel.SetActive(true);
            m_Panel.SetName("qwe");
        }

        public void Exit()
        {
            m_Panel.SetActive(false);
        }

        private void OnBackButtonClick()
        {
            m_MenuMachine.PopState();
        }

        private void OnCreateButtonClick()
        {
            var name = m_Panel.GetName();
            var status = m_RecordCreateStrategy.Create(name);
            if (status == GameRecordCreateStatus.Successful)
            {
                m_MenuMachine.PopState();
            }
            else
            {
                // todo
                HLogger.LogError("name is exists");
            }
        }

        public void Dispose()
        {
            
        }
    }
}