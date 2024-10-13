using System;

namespace App.Common.FSM.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class Stage : Attribute
    {
        private readonly string m_Name;
        private readonly int m_Order;
        
        public Stage(Type type, int order)
        {
            m_Name = type.Name;
            m_Order = order;
        }
        
        public Stage(string name, int order)
        {
            m_Name = name;
            m_Order = order;
        }
        
        public string GetName()
        {
            return m_Name;
        }
        
        public int GetOrder()
        {
            return m_Order;
        }
    }
}