using System;

namespace App.Common.Autumn.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class TransientAttribute : Attribute
    {
        
    }
}