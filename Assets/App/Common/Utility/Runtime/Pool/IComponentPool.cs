using UnityEngine;

namespace App.Common.Utility.Runtime.Pool
{
    public interface IComponentPool<T> : IPool<T> where T : Component
    {
        T Get(Transform parent);
        T Get(Transform parent, Vector3 position);
        T Get(Vector3 position);
    }
}