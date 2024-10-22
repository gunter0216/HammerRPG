using App.Common.AssemblyManager.Runtime;

namespace App.Common.AssemblyManager.External
{
    public class AssemblyManager
    {
        public AssemblyProviderBuilder CreateAssemblyProviderBuilder()
        {
            return new AssemblyProviderBuilder();
        }
    }
}