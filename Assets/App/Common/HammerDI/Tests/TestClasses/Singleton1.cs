using App.Common.HammerDI.Runtime.Attributes;

namespace App.Common.HammerDI.Tests.TestClasses
{
    [Singleton]
    public class Singleton1
    {
        public int GetValue()
        {
            return 10;
        }
    }
}