using System;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace App.Common.AssetSystem.Runtime
{
    public interface IContextInstanceAssetLoader : IAssetLoader
    {
        Optional<T> InstantiateSync<T>(IKeyEvaluator key, Transform parent = null, Type context = null)
            where T : Object;
        void UnloadContext(Type context);
    }
}