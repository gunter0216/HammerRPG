using System;
using App.Common.Utility.Pool.Runtime;

namespace App.Common.Utility.Pool.Tests.Items
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