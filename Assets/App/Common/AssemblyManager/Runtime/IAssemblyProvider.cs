using System;
using System.Collections.Generic;

namespace App.Common.AssemblyManager.Runtime
{
    public interface IAssemblyProvider
    {
        List<AttributeNode> GetTypes<T>() where T : Attribute;
    }
}