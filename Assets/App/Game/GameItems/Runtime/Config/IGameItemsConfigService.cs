using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.GameItems.Runtime.Config
{
    public interface IGameItemsConfigService
    {
        void SetItems(IReadOnlyList<IModuleItemConfig> items);
        Optional<IReadOnlyList<IModuleItemConfig>> GetItemsByType(string type);
    }
}