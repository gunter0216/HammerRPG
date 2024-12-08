using System;

namespace App.Common.HammerDI.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SingletonAttribute : Attribute
    {
        
    }
}