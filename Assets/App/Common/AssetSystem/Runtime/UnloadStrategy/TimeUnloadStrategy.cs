using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace App.Common.AssetSystem.Runtime.UnloadStrategy
{
    public class TimeUnloadStrategy : IUnloadStrategy
    {
        private readonly float _timeToUnload;

        public TimeUnloadStrategy(float timeToUnload)
        {
            _timeToUnload = timeToUnload;
        }

        public void UnloadUnusedAssets(Dictionary<IKeyEvaluator, AssetInfo> loadedAssets)
        {
            var removedAssets = new List<IKeyEvaluator>();
            foreach (var loadedAsset in loadedAssets)
            {
                var value = loadedAsset.Value;
                if (value.MarkToUnload)
                {
                    if (Time.realtimeSinceStartup - value.UsedLastTime > _timeToUnload)
                    {
                        Addressables.Release(value.Asset);
                        removedAssets.Add(loadedAsset.Key);
                    }
                }
            }

            foreach (var removedAsset in removedAssets)
            {
                loadedAssets.Remove(removedAsset);
            }
        }
    }
}