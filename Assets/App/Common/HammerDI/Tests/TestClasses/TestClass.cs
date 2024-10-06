using System.Collections.Generic;
using App.Common.HammerDI.Runtime.Attributes;

namespace App.Common.HammerDI.Tests.TestClasses
{
    [Scoped(typeof(TestContext))]
    public class TestClass
    {
        [Inject] private Class1Interface1 m_Class1Interface1;
        [Inject] private Class2Interface1 m_Class2Interface1;
        [Inject] private IInterface2 m_Interface2;
        [Inject] private List<IInterface1> m_Interface1s;
    }
}