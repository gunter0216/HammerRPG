using System;

namespace App.Common.Autumn.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ScopedAttribute : Attribute
    {
        public object Context { get; set; }

        public ScopedAttribute(object context)
        {
            Context = context;
        }
    }
}