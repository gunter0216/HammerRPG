using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime.DestroyStrategy;
using App.Common.Logger.Runtime;
using App.Common.Utility;
using App.Common.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace App.Common.AssetSystem.Runtime
{
    public class InstanceAssetLoader : IInstanceAssetLoader
    {
        private readonly Dictionary<IKeyEvaluator, HashSet<Object>> _instantiatedItems = new();

        private readonly IDestroyStrategy _destroyStrategy;
        private readonly IAssetLoader _assetLoader;

        public InstanceAssetLoader(IAssetLoader assetLoader, IDestroyStrategy destroyStrategy)
        {
            _destroyStrategy = destroyStrategy;
            _assetLoader = assetLoader;
        }
        
        public Optional<T> InstantiateSync<T>(IKeyEvaluator key, Transform parent = null) where T : Object
        {
            var asset = LoadSync<GameObject>(key);
            if (!asset.HasValue)
            {
                HLogger.LogError($"Cant load asset = {key}.");
                return Optional<T>.Empty;
            }

            var item = Object.Instantiate(asset.Value, parent);

            if (!_instantiatedItems.ContainsKey(key))
            {
                _instantiatedItems.Add(key, new HashSet<Object>(1));
            }

            _instantiatedItems[key].Add(item);
            
            return new Optional<T>(item.GetComponent<T>());
        }

        public void UnloadInstance(IKeyEvaluator key, Object item)
        {
            if (!_instantiatedItems.TryGetValue(key, out var hashSet))
            {
                HLogger.LogError($"Cant unload asset = {key}.");
                return;
            }

            hashSet.Remove(item);

            if (hashSet.Count <= 0)
            {
                MarkToUnload(key);
            }
            
            _destroyStrategy.Destroy(item);
        }

        public Optional<T> LoadSync<T>(IKeyEvaluator key) where T : Object
        {
            return _assetLoader.LoadSync<T>(key);
        }

        public void UnloadAsset(IKeyEvaluator key)
        {
            _assetLoader.UnloadAsset(key);
        }

        public void UnloadUnusedAssets()
        {
            _assetLoader.UnloadUnusedAssets();
        }

        public void MarkToUnload(IKeyEvaluator key)
        {
            _assetLoader.MarkToUnload(key);
        }
    }
}