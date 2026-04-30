using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Game.CameraFollow.External
{
    public class CameraController : IInitSystem
    {
        private readonly IAssetManager _assetManager;
        
        private GameObject _camera;

        public GameObject Camera => _camera;

        public CameraController(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        public void Init()
        {
            var cameraPrefab = _assetManager.LoadSync<GameObject>("Camera");
            if (!cameraPrefab.HasValue)
            {
                HLogger.LogError($"Cant create camera.");
                return;
            }
            
            _camera = Object.Instantiate(cameraPrefab.Value.gameObject);
            Object.DontDestroyOnLoad(_camera);
        }
    }
}