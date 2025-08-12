using System;
using System.Collections.Generic;
using App.Common.AssetSystem.Runtime.UnloadStrategy;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace App.Common.AssetSystem.Runtime
{
    public class AssetLoader : IAssetLoader
    {
        private readonly Dictionary<IKeyEvaluator, AssetInfo> _loadedAssets = new();

        private readonly IUnloadStrategy _unloadStrategy;

        public AssetLoader(IUnloadStrategy unloadStrategy)
        {
            _unloadStrategy = unloadStrategy;
        }

        public void UnloadUnusedAssets()
        {
            _unloadStrategy.UnloadUnusedAssets(_loadedAssets);
        }
        
        public Optional<T> LoadSync<T>(IKeyEvaluator key) where T : Object
        {
            if (TryGetAssetFromCache(key, out T asset))
            {
                return new Optional<T>(asset);
            }

            try
            {
                asset = Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
            }
            catch (Exception e)
            {
                HLogger.LogError($"Cant load asset {key}");
                // ignored
            }

            if (asset == default)
            {
                return Optional<T>.Empty;
            }

            CacheAsset(key, asset);

            return new Optional<T>(asset);
        }

        private void CacheAsset<T>(IKeyEvaluator key, T asset) where T : Object
        {
            var info = new AssetInfo()
            {
                Asset = asset,
                MarkToUnload = false,
                UsedLastTime = Time.realtimeSinceStartup
            };
            
            _loadedAssets.Add(key, info);
        }

        private bool TryGetAssetFromCache<T>(IKeyEvaluator key, out T returnAsset) where T : Object
        {
            if (_loadedAssets.TryGetValue(key, out var assetInfo))
            {
                assetInfo.MarkToUnload = false;
                assetInfo.UsedLastTime = Time.realtimeSinceStartup;
                returnAsset = assetInfo.Asset as T;
                return true;
            }

            returnAsset = null;

            return false;
        }

        public void MarkToUnload(IKeyEvaluator key)
        {
            if (!_loadedAssets.TryGetValue(key, out var assetInfo))
            {
                HLogger.LogError($"Cant unload asset, asset is not loaded.");
                return;
            }

            assetInfo.MarkToUnload = true;
            assetInfo.UsedLastTime = Time.realtimeSinceStartup;
        }
        
        public void UnloadAsset(IKeyEvaluator key)
        {
            if (!_loadedAssets.ContainsKey(key))
            {
                HLogger.LogError($"Cant unload asset, asset is not loaded.");
                return;
            }
        
            Addressables.Release(_loadedAssets[key].Asset);
        }
    }
}