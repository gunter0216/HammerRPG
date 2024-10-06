using System;

namespace App.Common.HammerDI.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ScopedAttribute : Attribute
    {
        public object Context { get; set; }

        public ScopedAttribute(object context)
        {
            Context = context;
        }
    }
}