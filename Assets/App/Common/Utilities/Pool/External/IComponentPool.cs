using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Common.Utilities.Pool.External
{
    public interface IComponentPool<T> : IPool<T> where T : Component
    {
        Optional<T> Get(Transform parent);
        Optional<T> Get(Transform parent, Vector3 position);
        Optional<T> Get(Vector3 position);
    }
}