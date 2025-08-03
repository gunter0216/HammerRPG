using System.Collections.Generic;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IModuleItemConfigController
    {
        Optional<IModuleItemConfig> GetConfig(string id);
        Optional<IReadOnlyList<IModuleItemConfig>> GetConfigs(string type);
    }
}