using System;
using App.Common.Utility.Pool.Runtime;

namespace App.Common.Utility.Pool.Tests.Items
{
    internal class ItemPoolGetListener : IPoolGetListener
    {
        private readonly Action m_OnGetFromPoolAction;

        public ItemPoolGetListener(Action onGetFromPool)
        {
            m_OnGetFromPoolAction = onGetFromPool;
        }

        public void OnGetFromPool()
        {
            m_OnGetFromPoolAction?.Invoke();
        }
    }
}