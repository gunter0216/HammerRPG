using App.Common.Autumn.Runtime.Attributes;

namespace App.Common.Autumn.Tests.TestClasses
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