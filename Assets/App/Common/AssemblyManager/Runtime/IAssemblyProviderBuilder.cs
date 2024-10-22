using System;

namespace App.Common.AssemblyManager.Runtime
{
    public interface IAssemblyProviderBuilder
    {
        IAssemblyProviderBuilder AddAttribute<T>() where T : Attribute;
        IAssemblyProvider Build();
    }
}