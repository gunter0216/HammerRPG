using System;
using App.Common.AssetSystem.Runtime;
using App.Common.AssetSystem.Runtime.DestroyStrategy;
using App.Common.AssetSystem.Runtime.UnloadStrategy;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Utility;
using App.Common.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace App.Common.AssetSystem.External
{
    [Singleton]
    public class AssetManager : IAssetManager
    {
        private IContextInstanceAssetLoader _contextInstanceAssetLoader;

        // todo добавить в конфигуратор
        public AssetManager()
        {
            var assetLoader = new AssetLoader(new TimeUnloadStrategy(3));
            var instanceAssetLoader = new InstanceAssetLoader(assetLoader, new SimpleDestroyStrategy());
            _contextInstanceAssetLoader = new ContextInstanceAssetLoader(instanceAssetLoader);
        }
        
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

        public Optional<T> LoadSync<T>(IKeyEvaluator key) where T : Object
        {
            return _contextInstanceAssetLoader.LoadSync<T>(key);
        }

        public void UnloadAsset(IKeyEvaluator key)
        {
            _contextInstanceAssetLoader.UnloadAsset(key);
        }

        public void UnloadContext(Type context)
        {
            _contextInstanceAssetLoader.UnloadContext(context);
        }
    }
}