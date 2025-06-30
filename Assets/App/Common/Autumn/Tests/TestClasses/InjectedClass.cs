using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Tests.TestClasses.Interfaces;

namespace App.Common.Autumn.Tests.TestClasses
{
    [Scoped(typeof(TestContext))]
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