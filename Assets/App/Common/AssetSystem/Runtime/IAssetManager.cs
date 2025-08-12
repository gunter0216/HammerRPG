using System;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace App.Common.AssetSystem.Runtime
{
    public interface IAssetManager
    {
        Optional<T> InstantiateSync<T>(IKeyEvaluator key, Transform parent = null, Type context = null) where T : Object;
        Optional<T> LoadSync<T>(IKeyEvaluator key) where T : Object;
        void UnloadAsset(IKeyEvaluator key);
    }
}