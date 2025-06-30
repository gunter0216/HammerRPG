using App.Common.AssetSystem.External;
using App.Common.AssetSystem.Runtime;
using App.Common.AssetSystem.Runtime.Context;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.Runtime;
using App.Common.SceneControllers.Runtime;
using App.Game.IconLoaders.External;
using UnityEngine.SceneManagement;

namespace App.Common.SceneControllers.External
{
    [Singleton]
    public class SceneManager : ISceneManager
    {
        [Inject] private AssetManager m_AssetManager;
        [Inject] private IconLoader m_IconLoader;
        [Inject] private IDataManager m_DataManager;
        
        public void LoadScene(string sceneName)
        {
            m_DataManager.SaveProgress();
            m_AssetManager.UnloadContext(typeof(SceneAssetContext));
            m_IconLoader.UnloadContextIcons();

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}