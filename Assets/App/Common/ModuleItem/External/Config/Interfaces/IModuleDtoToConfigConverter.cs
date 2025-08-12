using System;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.External.Config.Interfaces
{
    public interface IModuleDtoToConfigConverter
    {
        Optional<IModuleConfig> Convert(object moduleDto);
        string GetModuleKey();
        Type GetModuleDtoType();
    }
}