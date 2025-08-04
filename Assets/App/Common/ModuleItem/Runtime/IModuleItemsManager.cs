using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime
{
    public interface IModuleItemsManager
    {
        ModuleItemResult<IModuleItem> Create(DataReference dataReference);
        ModuleItemResult<IModuleItem> Create(string id);
        bool Destroy(IModuleItem data);
        Optional<IModuleItemConfig> GetConfig(string id);
        Optional<IReadOnlyList<IModuleItemConfig>> GetConfigs(string type);
    }
}