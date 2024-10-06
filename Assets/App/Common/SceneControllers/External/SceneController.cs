using App.Common.AssetSystem.External;
using App.Common.AssetSystem.Runtime;
using App.Common.AssetSystem.Runtime.Context;
using App.Common.HammerDI.Runtime.Attributes;
using UnityEngine.SceneManagement;

namespace App.Common.SceneControllers.External
{
    [Singleton]
    public class SceneController
    {
        [Inject] private AssetManager m_AssetManager;
        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            m_AssetManager.UnloadContext(typeof(SceneAssetContext));
        }
    }
}