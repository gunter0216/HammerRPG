using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace App.Common.Utility.Runtime.Pool
{
    public class Pool<TCollection, T> : IDisposable where TCollection : class, ICollection<T>, new()
    {
        private readonly Func<T> m_CreateFunc;
        private readonly Action<T> m_ActionOnGet;
        private readonly Action<T> m_ActionOnRelease;
        private readonly Action<T> m_ActionOnDestroy;

        private readonly TCollection m_Items;
        private readonly TCollection m_ActiveItems;

        public Pool(
            Func<T> createFunc, 
            Action<T> actionOnGet = null, 
            Action<T> actionOnRelease = null, 
            Action<T> actionOnDestroy = null)
        {
            m_CreateFunc = createFunc;
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
            m_ActionOnDestroy = actionOnDestroy;
            m_Items = new TCollection();
            m_ActiveItems = new TCollection();
        }

        public T Get()
        {
            T item;
            if (m_Items.Count > 0)
            {
                item = m_Items.Last();
                m_Items.Remove(item);
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

        public void Dispose()
        {
            if (m_ActionOnDestroy == null)
            {
                return;
            }

            foreach (var item in m_Items)
            {
                m_ActionOnDestroy.Invoke(item);
            }
            
            foreach (var item in m_ActiveItems)
            {
                m_ActionOnDestroy.Invoke(item);
            }
        }
    }
}