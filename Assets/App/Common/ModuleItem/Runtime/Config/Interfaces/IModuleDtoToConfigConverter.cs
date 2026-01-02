using System;
using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IModuleDtoToConfigConverter
    {
        Optional<IModuleConfig> Convert(Dictionary<string, string> module);
        string GetModuleKey();
        Type GetModuleDtoType();
    }
}