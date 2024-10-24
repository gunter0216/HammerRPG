using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Common.Utility.Runtime.Pool
{
    public class ComponentPool<T> : IComponentPool<T>, IDisposable where T : Component
    {
        private readonly ListPool<T> m_Pool;

        public int Capacity => m_Pool.Capacity;
        public IReadOnlyCollection<T> ActiveItems => m_Pool.ActiveItems;
        
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
                    return item;
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

        public T Get()
        {
            return m_Pool.Get();
        }

        public T Get(Transform parent)
        {
            var item = m_Pool.Get();
            item.transform.SetParent(parent);
            
            return item;
        }

        public T Get(Transform parent, Vector3 position)
        {
            var item = m_Pool.Get();
            item.transform.SetParent(parent);
            item.transform.position = position;
            
            return item;
        }

        public T Get(Vector3 position)
        {
            var item = m_Pool.Get();
            item.transform.position = position;
            
            return item;
        }

        public void Release(T item)
        {
            m_Pool.Release(item);
        }

        public void ReleaseAll()
        {
            m_Pool.ReleaseAll();
        }

        public void Dispose()
        {
            m_Pool?.Dispose();
        }
    }
}