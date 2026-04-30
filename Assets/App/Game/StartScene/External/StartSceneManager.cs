using App.Common.SceneControllers.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Game.StartScene.External
{
    public class StartSceneManager : IInitSystem
    {
        private readonly ISceneManager m_SceneManager;

        public StartSceneManager(ISceneManager sceneManager)
        {
            m_SceneManager = sceneManager;
        }

        public void Init()
        {
            m_SceneManager.LoadScene(SceneConstants.MenuScene);
        }
    }
}