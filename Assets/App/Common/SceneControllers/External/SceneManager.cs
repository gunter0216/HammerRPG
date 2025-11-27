using App.Common.AssetSystem.External;
using App.Common.AssetSystem.Runtime.Context;
using App.Common.Data.Runtime;
using App.Common.SceneControllers.Runtime;
using App.Common.SpriteLoaders.External;

namespace App.Common.SceneControllers.External
{
    public class SceneManager : ISceneManager
    {
        private readonly AssetManager m_AssetManager;
        private readonly SpriteLoader m_SpriteLoader;
        private readonly IDataManager m_DataManager;

        public SceneManager(AssetManager assetManager, SpriteLoader spriteLoader, IDataManager dataManager)
        {
            m_AssetManager = assetManager;
            m_SpriteLoader = spriteLoader;
            m_DataManager = dataManager;
        }

        public void LoadScene(string sceneName)
        {
            m_DataManager.SaveProgress();
            m_AssetManager.UnloadContext(typeof(SceneAssetContext));
            m_SpriteLoader.UnloadContextIcons();

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}