using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace App.Common.AssetSystem.Runtime
{
    public interface IAssetLoader
    {
        Optional<T> LoadSync<T>(IKeyEvaluator key) where T : Object;
        void UnloadAsset(IKeyEvaluator key);
        void UnloadUnusedAssets();
        void MarkToUnload(IKeyEvaluator key);
    }
}