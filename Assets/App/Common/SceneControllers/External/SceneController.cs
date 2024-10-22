using App.Common.AssetSystem.External;
using App.Common.AssetSystem.Runtime;
using App.Common.AssetSystem.Runtime.Context;
using App.Common.HammerDI.Runtime.Attributes;
using App.Game.IconLoaders.External;
using UnityEngine.SceneManagement;

namespace App.Common.SceneControllers.External
{
    [Singleton]
    public class SceneController
    {
        [Inject] private AssetManager m_AssetManager;
        [Inject] private IconLoader m_IconLoader;
        
        public void LoadScene(string sceneName)
        {
            m_AssetManager.UnloadContext(typeof(SceneAssetContext));
            m_IconLoader.UnloadContextIcons();
            
            SceneManager.LoadScene(sceneName);
        }
    }
}