using App.Common.SceneControllers.Runtime;
using App.Game.Pause.Runtime;
using UnityEngine;

namespace App.Game.GameMenu.Runtime
{
    public class SaveAndExitStrategy : ISaveGameStrategy
    {
        private readonly ISceneManager m_SceneManager;
        private readonly IPauseController m_PauseController; 

        public SaveAndExitStrategy(ISceneManager sceneManager, IPauseController pauseController)
        {
            m_SceneManager = sceneManager;
            m_PauseController = pauseController;
        }

        public void Save()
        {
            // todo
            m_PauseController.Unpause();
            m_SceneManager.LoadScene(SceneConstants.MenuScene);
        }
    }
}