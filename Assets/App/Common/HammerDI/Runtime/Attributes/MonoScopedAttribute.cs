using System;

namespace App.Common.HammerDI.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class MonoScopedAttribute : Attribute
    {
        
    }
}