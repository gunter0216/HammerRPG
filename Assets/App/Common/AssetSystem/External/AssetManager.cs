using System;
using App.Common.AssetSystem.Runtime;
using App.Common.AssetSystem.Runtime.DestroyStrategy;
using App.Common.AssetSystem.Runtime.UnloadStrategy;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace App.Common.AssetSystem.External
{
    public class AssetManager : IAssetManager
    {
        private readonly IContextInstanceAssetLoader m_ContextInstanceAssetLoader;

        public AssetManager()
        {
            var assetLoader = new AssetLoader(new TimeUnloadStrategy(3));
            var instanceAssetLoader = new InstanceAssetLoader(assetLoader, new SimpleDestroyStrategy());
            m_ContextInstanceAssetLoader = new ContextInstanceAssetLoader(instanceAssetLoader);
        }
        
        public Optional<T> InstantiateSync<T>(
            IKeyEvaluator key, 
            Transform parent = null,
            Type context = null)
            where T : Object
        {
            return m_ContextInstanceAssetLoader.InstantiateSync<T>(key, parent, context);
        }

        public Optional<T> LoadSync<T>(IKeyEvaluator key) where T : Object
        {
            return m_ContextInstanceAssetLoader.LoadSync<T>(key);
        }

        public void UnloadAsset(IKeyEvaluator key)
        {
            m_ContextInstanceAssetLoader.UnloadAsset(key);
        }

        public void UnloadContext(Type context)
        {
            m_ContextInstanceAssetLoader.UnloadContext(context);
        }
    }
}