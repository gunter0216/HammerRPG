using App.Common.Autumn.Runtime.Attributes;
using App.Common.Autumn.Tests.TestClasses.Interfaces;

namespace App.Common.Autumn.Tests.TestClasses
{
    [Scoped(typeof(TestContext))]
    public class Class2Interface1 : IInterface1
    {
        [Inject] private IInjectedClass _injectedClass;

        public int GetValue()
        {
            return _injectedClass.GetValue();
        }
    }
}