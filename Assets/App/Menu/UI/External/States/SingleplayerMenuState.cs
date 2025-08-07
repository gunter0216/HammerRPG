using System;
using System.Collections.Generic;
using App.Common.Timer.Runtime;
using App.Common.Utility.Pool.External;
using App.Game.Utility.Runtime.MenuSM;
using App.Menu.UI.Runtime.Data;
using App.Menu.UI.Runtime.View.Panels.Singleplayer;

namespace App.Menu.UI.Runtime.States
{
    public class SingleplayerMenuState : IMenuState, IDisposable
    {
        private readonly MenuMachine m_MenuMachine;
        private readonly SingleplayerPanel m_SingleplayerPanel;
        private readonly CreateGameMenuState m_CreateGameMenuState;
        private readonly GameRecordsDataController m_GameRecordsDataController;
        private readonly IStartGameStrategy m_StartGameStrategy;

        private readonly ITimeToStringConverter m_TimeToStringConverter;
        private readonly ComponentPool<SingleplayerSaveRow> m_Pool;
        
        private List<SingleplayerSaveRow> m_ActiveRows;
        
        public SingleplayerMenuState(MenuMachine menuMachine, 
            SingleplayerPanel singleplayerPanel, 
            CreateGameMenuState createGameMenuState, 
            GameRecordsDataController gameRecordsDataController, 
            IStartGameStrategy startGameStrategy)
        {
            m_SingleplayerPanel = singleplayerPanel;
            m_CreateGameMenuState = createGameMenuState;
            m_GameRecordsDataController = gameRecordsDataController;
            m_StartGameStrategy = startGameStrategy;
            m_MenuMachine = menuMachine;

            m_ActiveRows = new List<SingleplayerSaveRow>();
            m_TimeToStringConverter = new TimeToStringConverter();
            m_Pool = new ComponentPool<SingleplayerSaveRow>(
                m_SingleplayerPanel.SaveRowPrefab,
                m_SingleplayerPanel.SaveRowsContent);
            
            m_SingleplayerPanel.SetActive(false);
            
            m_SingleplayerPanel.SubscribeToBackButtonClick(OnBackButtonClick);
            m_SingleplayerPanel.SetCreateNewGameButtonAction(OnCreateNewGameButtonClick);
        }

        private void OnCreateNewGameButtonClick()
        {
            m_MenuMachine.PushState(m_CreateGameMenuState);
        }

        public void Enter()
        {
            m_SingleplayerPanel.SetActive(true);

            foreach (var activeRow in m_ActiveRows)
            {
                m_Pool.Release(activeRow);
            }
            
            var records = m_GameRecordsDataController.GetRecords();
            records.Sort((a, b) => b.LastLogin.CompareTo(a.LastLogin));
            foreach (var gameRecord in records)
            {
                var view = m_Pool.Get();
                m_ActiveRows.Add(view.Value);
                view.Value.SetCreateDate( "created " + m_TimeToStringConverter.ReturnTimeToShow(gameRecord.DateOfCreation));
                view.Value.SetLastLoginText("login " + m_TimeToStringConverter.ReturnTimeToShow(gameRecord.LastLogin));
                view.Value.SetNameText(gameRecord.Name);
                view.Value.SetPlayButtonAction(() =>
                {
                    m_StartGameStrategy.StartGame(gameRecord.Name);
                });
            }
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
            m_Pool?.Dispose();
        }
    }
}