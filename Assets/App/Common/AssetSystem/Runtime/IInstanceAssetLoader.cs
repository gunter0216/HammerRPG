using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace App.Common.AssetSystem.Runtime
{
    public interface IInstanceAssetLoader : IAssetLoader
    {
        
        Optional<T> InstantiateSync<T>(IKeyEvaluator key, Transform parent = null) where T : Object;
        void UnloadInstance(IKeyEvaluator key, Object item);
    }
}