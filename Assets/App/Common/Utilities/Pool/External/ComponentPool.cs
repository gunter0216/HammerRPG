using System;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Common.Utilities.Pool.External
{
    public class ComponentPool<T> : IComponentPool<T>, IDisposable where T : Component
    {
        private readonly ListPool<T> m_Pool;

        public int Capacity => m_Pool.Capacity;
        
        public ComponentPool(
            T prefab,
            Transform parent = null,
            int capacity = 0,
            Action<T> onCreate = null,
            Action<T> onGet = null,
            Action<T> onRelease = null,
            Action<T> onDestroy = null)
        {
            m_Pool = new ListPool<T>(
                createFunc: () =>
                {
                    var item = Object.Instantiate(prefab, parent);
                    onCreate?.Invoke(item);
                    return Optional<T>.Success(item);
                },
                actionOnGet: (item) =>
                {
                    item.gameObject.SetActive(true);
                    onGet?.Invoke(item);
                },
                actionOnRelease: (item) =>
                {
                    item.gameObject.SetActive(false);
                    onRelease?.Invoke(item);
                },
                actionOnDestroy: (item) =>
                {
                    onDestroy?.Invoke(item);
                },
                capacity: capacity);
        }

        public Optional<T> Get()
        {
            return m_Pool.Get();
        }

        public Optional<T> Get(Transform parent)
        {
            var item = m_Pool.Get();
            if (!item.HasValue)
            {
                return Optional<T>.Fail();
            }
            
            item.Value.transform.SetParent(parent);
            
            return item;
        }

        public Optional<T> Get(Transform parent, Vector3 position)
        {
            var item = m_Pool.Get();
            if (!item.HasValue)
            {
                return Optional<T>.Fail();
            }
            
            item.Value.transform.SetParent(parent);
            item.Value.transform.position = position;
            
            return item;
        }

        public Optional<T> Get(Vector3 position)
        {
            var item = m_Pool.Get();
            if (!item.HasValue)
            {
                return Optional<T>.Fail();
            }
            
            item.Value.transform.position = position;
            
            return item;
        }

        public bool Release(T item)
        {
            return m_Pool.Release(item);
        }

        public void Dispose()
        {
            m_Pool?.Dispose();
        }
    }
}