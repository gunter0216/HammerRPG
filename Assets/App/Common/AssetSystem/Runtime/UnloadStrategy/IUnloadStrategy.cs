using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace App.Common.AssetSystem.Runtime.UnloadStrategy
{
    public interface IUnloadStrategy
    {
        void UnloadUnusedAssets(Dictionary<IKeyEvaluator, AssetInfo> loadedAssets);
    }
}