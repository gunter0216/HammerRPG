using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Game.GameItems.Runtime
{
    public interface IGameItemsManager
    {
        ModuleItemResult<IGameModuleItem> Create(DataReference dataReference);
        ModuleItemResult<IGameModuleItem> Create(string id);
        bool Destroy(IGameModuleItem data);
        Optional<IReadOnlyList<IModuleItemConfig>> GetItemsByType(string type);
    }
}