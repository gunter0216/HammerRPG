using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace Assets.App.Common.ModuleItem.Runtime
{
    public interface IModuleItemsManager
    {
        Optional<IModuleItem> Create(DataReference dataReference);
        Optional<IModuleItem> Create(string id);
        bool Destroy(IModuleItem data);
        Optional<IModuleItemConfig> GetConfig(string id);
        Optional<IReadOnlyList<IModuleItemConfig>> GetConfigs(string type);
    }
}