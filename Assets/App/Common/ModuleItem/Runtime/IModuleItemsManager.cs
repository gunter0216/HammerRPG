using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime
{
    public interface IModuleItemsManager
    {
        bool RegisterItems(IModuleItemsConfigLoader moduleItemsConfigLoader, string type);
        bool RegisterItems(IReadOnlyList<IModuleItemConfig> configs, string type);
        Optional<IModuleItem> Create(DataReference dataReference);
        Optional<IModuleItem> Create(string id);
        bool Destroy(IModuleItem data);
        Optional<IModuleItemConfig> GetConfig(string id);
        Optional<IReadOnlyList<IModuleItemConfig>> GetConfigs(string type);
    }
}