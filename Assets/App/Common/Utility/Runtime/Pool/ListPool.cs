using System;
using System.Collections.Generic;

namespace App.Common.Utility.Runtime.Pool
{
    public class ListPool<T> : IListPool<T>, IDisposable
    {
        private readonly Func<T> m_CreateFunc;
        private readonly Action<T> m_ActionOnGet;
        private readonly Action<T> m_ActionOnRelease;
        private readonly Action<T> m_ActionOnDestroy;

        private readonly List<T> m_Items;
        private readonly List<T> m_ActiveItems;

        public int Capacity => m_Items.Count + m_ActiveItems.Count;

        public IReadOnlyList<T> ActiveItems => m_ActiveItems;

        IReadOnlyCollection<T> IPool<T>.ActiveItems => m_ActiveItems;

        public ListPool(
            Func<T> createFunc, 
            Action<T> actionOnGet = null, 
            Action<T> actionOnRelease = null, 
            Action<T> actionOnDestroy = null,
            int capacity = 0)
        {
            m_CreateFunc = createFunc;
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
            m_ActionOnDestroy = actionOnDestroy;
            m_Items = new List<T>(capacity);
            m_ActiveItems = new List<T>();
            
            if (capacity > 0)
            {
                for (int i = 0; i < capacity; ++i)
                {
                    m_Items.Add(m_CreateFunc.Invoke());
                }
            }
        }

        public T Get()
        {
            T item;
            if (m_Items.Count > 0)
            {
                item = m_Items[^1];
                m_Items.RemoveAt(m_Items.Count - 1);
            }
            else
            {
                item = m_CreateFunc.Invoke();
            }
            
            m_ActiveItems.Add(item);
            m_ActionOnGet?.Invoke(item);
            
            return item;
        }

        public void Release(T item)
        {
            m_ActiveItems.Remove(item);
            m_Items.Add(item);
            m_ActionOnRelease?.Invoke(item);
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < m_ActiveItems.Count; ++i)
            {
                m_ActionOnRelease?.Invoke(m_ActiveItems[i]);
            }
            
            m_Items.AddRange(m_ActiveItems);
            
            m_ActiveItems.Clear();
        }

        public void Dispose()
        {
            if (m_ActionOnDestroy == null)
            {
                return;
            }

            for (int i = 0; i < m_Items.Count; ++i)
            {
                m_ActionOnDestroy.Invoke(m_Items[i]);
            }
            
            for (int i = 0; i < m_ActiveItems.Count; ++i)
            {
                m_ActionOnDestroy.Invoke(m_ActiveItems[i]);
            }
        }
    }
}