using System;
using App.Common.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace App.Common.AssetSystem.Runtime
{
    public interface IAssetManager
    {
        Optional<T> InstantiateSync<T>(IKeyEvaluator key, Transform parent = null, Type context = null) where T : Object;
    }
}