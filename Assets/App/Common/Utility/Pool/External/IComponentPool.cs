using App.Common.Utility.Pool.Runtime;
using UnityEngine;

namespace App.Common.Utility.Pool.External
{
    public interface IComponentPool<T> : IPool<T> where T : Component
    {
        PoolItemHolder<T> Get(Transform parent);
        PoolItemHolder<T> Get(Transform parent, Vector3 position);
        PoolItemHolder<T> Get(Vector3 position);
    }
}