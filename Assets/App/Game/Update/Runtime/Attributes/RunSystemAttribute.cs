using System;

namespace App.Game.Update.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class RunSystemAttribute : Attribute
    {
        private readonly int m_Order;
        
        public RunSystemAttribute(int order)
        {
            m_Order = order;
        }
        
        public int GetOrder()
        {
            return m_Order;
        }
    }
}