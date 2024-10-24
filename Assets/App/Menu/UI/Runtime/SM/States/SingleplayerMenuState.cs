using System;
using App.Common.SceneControllers.Runtime;
using App.Common.Utility.Runtime.Pool;
using App.Common.Utility.Runtime.Time;
using App.Menu.UI.External.View.Panels;
using App.Menu.UI.External.View.Panels.Singleplayer;
using App.Menu.UI.Runtime.Data;
using UnityEngine;

namespace App.Menu.UI.External.FSM.States
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
            
            m_Pool.ReleaseAll();
            var records = m_GameRecordsDataController.GetRecords();
            records.Sort((a, b) => b.LastLogin.CompareTo(a.LastLogin));
            foreach (var gameRecord in records)
            {
                var view = m_Pool.Get();
                view.SetCreateDate( "created " + m_TimeToStringConverter.ReturnTimeToShow(gameRecord.DateOfCreation));
                view.SetLastLoginText("login " + m_TimeToStringConverter.ReturnTimeToShow(gameRecord.LastLogin));
                view.SetNameText(gameRecord.Name);
                view.SetPlayButtonAction(() =>
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