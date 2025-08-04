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
        
        private readonly Action<PoolItemHolder<T>> m_ActionOnCreateSuccessful;

        private readonly List<PoolItemHolder<T>> m_Items;
        private readonly List<PoolItemHolder<T>> m_ActiveItems;

        public int Capacity => m_Items.Count + m_ActiveItems.Count;
        public IReadOnlyList<PoolItemHolder<T>> ActiveItems => m_ActiveItems;
        public int CountActiveItems => m_ActiveItems.Count;

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
            m_Items = new List<PoolItemHolder<T>>(capacity);
            m_ActiveItems = new List<PoolItemHolder<T>>();

            if (typeof(IPoolItem).IsAssignableFrom(typeof(T)))
            {
                m_ActionOnCreateSuccessful = itemHolder => ((IPoolItem)itemHolder.Item).ReturnInPool = () => Release(itemHolder);
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
                        var itemHolder = new PoolItemHolder<T>()
                        {
                            Item = item.Value,
                            IsActive = false
                        };
                        m_Items.Add(itemHolder);
                        m_ActionOnCreateSuccessful?.Invoke(itemHolder);
                    }
                }
            }
        }

        public Optional<PoolItemHolder<T>> Get()
        {
            if (m_ActiveItems.Count >= m_MaxItems)
            {
                return Optional<PoolItemHolder<T>>.Fail();
            }
            
            PoolItemHolder<T> itemHolder;
            if (m_Items.Count > 0)
            {
                itemHolder = m_Items[^1];
                m_Items.RemoveAt(m_Items.Count - 1);
            }
            else
            {
                var itemResult = m_CreateFunc.Invoke();
                if (itemResult.HasValue)
                {
                    itemHolder = new PoolItemHolder<T>()
                    {
                        Item = itemResult.Value
                    };
                    
                    m_ActionOnCreateSuccessful?.Invoke(itemHolder);
                }
                else
                {
                    return Optional<PoolItemHolder<T>>.Fail();
                }
            }

            itemHolder.IsActive = true;
            m_ActiveItems.Add(itemHolder);
            m_ActionOnGet?.Invoke(itemHolder.Item);
            
            return Optional<PoolItemHolder<T>>.Success(itemHolder);
        }

        public bool Release(PoolItemHolder<T> itemHolder)
        {
            if (!m_ActiveItems.Remove(itemHolder))
            {
                return false;
            }

            itemHolder.IsActive = false;
            m_Items.Add(itemHolder);
            m_ActionOnRelease?.Invoke(itemHolder.Item);
            
            return true;
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < m_ActiveItems.Count; ++i)
            {
                var itemHolder = m_ActiveItems[i];
                itemHolder.IsActive = false;
                m_ActionOnRelease?.Invoke(itemHolder.Item);
            }
            
            m_Items.AddRange(m_ActiveItems);
            
            m_ActiveItems.Clear();
        }

        public void Dispose()
        {
            if (m_ActionOnDestroy != null)
            {
                for (int i = 0; i < m_Items.Count; ++i)
                {
                    m_ActionOnDestroy.Invoke(m_Items[i].Item);
                }
            
                for (int i = 0; i < m_ActiveItems.Count; ++i)
                {
                    m_ActionOnDestroy.Invoke(m_ActiveItems[i].Item);
                }
            }

            m_Items.Clear();
            m_ActiveItems.Clear();
        }
    }
}