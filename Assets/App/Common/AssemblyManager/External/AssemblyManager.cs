using App.Common.AssemblyManager.Runtime;
using App.Common.Logger.Runtime;

namespace App.Common.AssemblyManager.External
{
    public class AssemblyManager
    {
        public AssemblyProviderBuilder CreateAssemblyProviderBuilder()
        {
            return new AssemblyProviderBuilder(new []
            {
                "Autumn.Game",
                "Autumn.Core"
            });
        }
    }
}