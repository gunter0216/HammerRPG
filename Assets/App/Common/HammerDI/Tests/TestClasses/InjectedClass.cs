using System.Security.Policy;
using App.Common.HammerDI.Runtime;
using App.Common.HammerDI.Runtime.Attributes;
using UnityEngine;

namespace App.Common.HammerDI.Tests.TestClasses
{
    [Scoped]
    public class InjectedClass : IInjectedClass
    {
        private int _value;

        public void SetValue(int value)
        {
            _value = value;
        }

        public int GetValue()
        {
            return _value;
        }
    }
}