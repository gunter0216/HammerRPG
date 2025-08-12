using System;
using App.Common.Utilities.Utility.Runtime;

namespace Assets.App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IModuleDtoToConfigConverter
    {
        Optional<IModuleConfig> Convert(object moduleDto);
        string GetModuleKey();
        Type GetModuleDtoType();
    }
}