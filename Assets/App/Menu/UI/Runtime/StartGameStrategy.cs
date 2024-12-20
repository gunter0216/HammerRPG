﻿using App.Common.SceneControllers.Runtime;
using App.Common.Timer.Runtime;
using App.Menu.UI.Runtime.Data;

namespace App.Menu.UI.Runtime
{
    public class StartGameStrategy : IStartGameStrategy
    {
        private readonly ISceneManager m_SceneManager;
        private readonly GameRecordsDataController m_DataController;

        public StartGameStrategy(ISceneManager sceneManager, GameRecordsDataController dataController)
        {
            m_SceneManager = sceneManager;
            m_DataController = dataController;
        }

        public void StartGame(string name)
        {
            m_DataController.SetLastLogin(name, TimeHelper.Now.Ticks);
            m_SceneManager.LoadScene(SceneConstants.GameScene);
        }
    }
}