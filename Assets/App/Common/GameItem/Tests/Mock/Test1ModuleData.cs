using App.Common.GameItem.Runtime.Data;

namespace App.Common.GameItem.Tests.Mock
{
    public class Test1ModuleData : IModuleData
    {
        public string GetModuleKey()
        {
            return "Test1";
        }
    }
}