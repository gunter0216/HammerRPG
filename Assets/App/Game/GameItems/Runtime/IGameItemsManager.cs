using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.ModuleItem.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;

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