using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Tests.TestClasses.Interfaces;

namespace App.Common.Autumn.Tests.TestClasses
{
    [Scoped(typeof(TestContext))]
    public class Class1Interface1 : IInterface1
    {
        [Inject] private InjectedClass _injectedClass;

        public int GetValue()
        {
            return _injectedClass.GetValue();
        }
    }
}