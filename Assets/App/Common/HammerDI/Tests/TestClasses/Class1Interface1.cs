using App.Common.HammerDI.Runtime;
using App.Common.HammerDI.Runtime.Attributes;

namespace App.Common.HammerDI.Tests.TestClasses
{
    [Scoped]
    public class Class1Interface1 : IInterface1
    {
        [Inject] private InjectedClass _injectedClass;

        public int GetValue()
        {
            return _injectedClass.GetValue();
        }
    }
}