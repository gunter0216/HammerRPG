using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace Assets.App.Game.GameItems.Runtime
{
    public interface IGameItemsManager
    {
        Optional<IGameModuleItem> Create(DataReference dataReference);
        Optional<IGameModuleItem> Create(string id);
        bool Destroy(IGameModuleItem data);
        Optional<IReadOnlyList<IModuleItemConfig>> GetItemsByType(string type);
    }
}