using App.Common.HammerDI.Runtime;
using App.Common.HammerDI.Runtime.Attributes;

namespace App.Common.HammerDI.Tests.TestClasses
{
    [Scoped]
    public class Class2Interface1 : IInterface1
    {
        [Inject] private IInjectedClass _injectedClass;

        public int GetValue()
        {
            return _injectedClass.GetValue();
        }
    }
}