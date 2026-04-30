using System;
using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Utilities.Pool.Runtime
{
    public class ListPool<T> : IPool<T>, IDisposable
    {
        private readonly Func<Optional<T>> m_CreateFunc;
        private readonly Action<T> m_GetCallback;
        private readonly Action<T> m_ReleaseCallback;
        
        private readonly Action<T> m_CreateSuccessfulCallback;

        private readonly List<T> m_Items;

        public int Capacity => m_Items.Count;

        public ListPool(
            Func<Optional<T>> createFunc, 
            int capacity = 0,
            Action<T> getCallback = null, 
            Action<T> releaseCallback = null)
        {
            m_CreateFunc = createFunc;
            m_GetCallback = getCallback;
            m_ReleaseCallback = releaseCallback;
            m_Items = new List<T>(capacity);

            if (typeof(IPoolItem).IsAssignableFrom(typeof(T)))
            {
                m_CreateSuccessfulCallback = itemHolder => ((IPoolItem)itemHolder).ReturnInPool = () => Release(itemHolder);
            }
            
            if (typeof(IPoolGetListener).IsAssignableFrom(typeof(T)))
            {
                m_GetCallback += item => ((IPoolGetListener)item).OnGetFromPool();
            }
            
            if (typeof(IPoolReleaseListener).IsAssignableFrom(typeof(T)))
            {
                m_ReleaseCallback += item => ((IPoolReleaseListener)item).BeforeReturnInPool();
            }

            if (capacity > 0)
            {
                for (int i = 0; i < capacity; ++i)
                {
                    var item = m_CreateFunc.Invoke();
                    if (item.HasValue)
                    {
                        m_Items.Add(item.Value);
                        m_CreateSuccessfulCallback?.Invoke(item.Value);
                    }
                }

                for (int i = 0; i < m_Items.Count; ++i)
                {
                    m_ReleaseCallback?.Invoke(m_Items[i]);
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
                    m_CreateSuccessfulCallback?.Invoke(itemResult.Value);
                }
                else
                {
                    return Optional<T>.Fail();
                }
            }

            m_GetCallback?.Invoke(item);
            
            return Optional<T>.Success(item);
        }

        public bool Release(T item)
        {
            m_Items.Add(item);
            m_ReleaseCallback?.Invoke(item);
            
            return true;
        }

        public void Dispose()
        {
            if (m_ReleaseCallback != null)
            {
                for (int i = 0; i < m_Items.Count; ++i)
                {
                    m_ReleaseCallback.Invoke(m_Items[i]);
                }
            }

            m_Items.Clear();
        }
    }
}