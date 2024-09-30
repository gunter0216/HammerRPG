using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Utility;
using App.Common.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace App.Common.AssetSystem.External
{
    public class AssetManager : IAssetManager
    {
        private IContextInstanceAssetLoader _contextInstanceAssetLoader;

        public AssetManager(IContextInstanceAssetLoader contextInstanceAssetLoader)
        {
            _contextInstanceAssetLoader = contextInstanceAssetLoader;
        }

        public Optional<T> InstantiateSync<T>(
            IKeyEvaluator key, 
            Transform parent = null,
            Type context = null)
            where T : Object
        {
            return _contextInstanceAssetLoader.InstantiateSync<T>(key, parent, context);
        }

        public void UnloadContext(Type context)
        {
            _contextInstanceAssetLoader.UnloadContext(context);
        }
    }
}