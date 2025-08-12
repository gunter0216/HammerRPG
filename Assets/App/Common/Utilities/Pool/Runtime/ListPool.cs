using System;
using System.Collections.Generic;
using App.Common.Utility.Runtime;

namespace App.Common.Utility.Pool.Runtime
{
    public class ListPool<T> : IPool<T>, IDisposable
    {
        private readonly int m_MaxItems;
        private readonly Func<Optional<T>> m_CreateFunc;
        private readonly Action<T> m_ActionOnGet;
        private readonly Action<T> m_ActionOnRelease;
        private readonly Action<T> m_ActionOnDestroy;
        
        private readonly Action<T> m_ActionOnCreateSuccessful;

        private readonly List<T> m_Items;

        public int Capacity => m_Items.Count;

        public ListPool(
            Func<Optional<T>> createFunc, 
            int capacity = 0,
            int maxItems = 100,
            Action<T> actionOnGet = null, 
            Action<T> actionOnRelease = null, 
            Action<T> actionOnDestroy = null)
        {
            m_CreateFunc = createFunc;
            m_MaxItems = maxItems;
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
            m_ActionOnDestroy = actionOnDestroy;
            m_Items = new List<T>(capacity);

            if (typeof(IPoolItem).IsAssignableFrom(typeof(T)))
            {
                m_ActionOnCreateSuccessful = itemHolder => ((IPoolItem)itemHolder).ReturnInPool = () => Release(itemHolder);
            }
            
            if (typeof(IPoolGetListener).IsAssignableFrom(typeof(T)))
            {
                m_ActionOnGet += item => ((IPoolGetListener)item).OnGetFromPool();
            }
            
            if (typeof(IPoolReleaseListener).IsAssignableFrom(typeof(T)))
            {
                m_ActionOnRelease += item => ((IPoolReleaseListener)item).BeforeReturnInPool();
            }

            if (capacity > 0)
            {
                for (int i = 0; i < capacity; ++i)
                {
                    var item = m_CreateFunc.Invoke();
                    if (item.HasValue)
                    {
                        m_Items.Add(item.Value);
                        m_ActionOnCreateSuccessful?.Invoke(item.Value);
                    }
                }

                for (int i = 0; i < m_Items.Count; ++i)
                {
                    m_ActionOnRelease?.Invoke(m_Items[i]);
                }
            }
        }

        public Optional<T> Get()
        {
            T item;
            if (m_Items.Count > 0)
            {
                item = m_Items[^1];
                m_Items.RemoveAt(m_Items.Count - 1);
            }
            else
            {
                var itemResult = m_CreateFunc.Invoke();
                if (itemResult.HasValue)
                {
                    item = itemResult.Value;
                    m_ActionOnCreateSuccessful?.Invoke(itemResult.Value);
                }
                else
                {
                    return Optional<T>.Fail();
                }
            }

            m_ActionOnGet?.Invoke(item);
            
            return Optional<T>.Success(item);
        }

        public bool Release(T item)
        {
            m_Items.Add(item);
            m_ActionOnRelease?.Invoke(item);
            
            return true;
        }

        public void Dispose()
        {
            if (m_ActionOnDestroy != null)
            {
                for (int i = 0; i < m_Items.Count; ++i)
                {
                    m_ActionOnDestroy.Invoke(m_Items[i]);
                }
            }

            m_Items.Clear();
        }
    }
}