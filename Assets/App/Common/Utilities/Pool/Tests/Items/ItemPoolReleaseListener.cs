using System;
using App.Common.Utilities.Pool.Runtime;

namespace App.Common.Utilities.Pool.Tests.Items
{
    internal class ItemPoolReleaseListener : IPoolReleaseListener
    {
        private readonly Action m_BeforeReturnInPoolAction;

        public ItemPoolReleaseListener(Action beforeReturnInPoolAction)
        {
            m_BeforeReturnInPoolAction = beforeReturnInPoolAction;
        }
        
        public void BeforeReturnInPool()
        {
            m_BeforeReturnInPoolAction?.Invoke();
        }
    }
}