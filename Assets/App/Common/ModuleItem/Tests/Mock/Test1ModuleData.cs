using Assets.App.Common.ModuleItem.Runtime.Data;

namespace App.Common.ModuleItem.Tests.Mock
{
    public class Test1ModuleData : IModuleData
    {
        public string GetModuleKey()
        {
            return "Test1";
        }
    }
}