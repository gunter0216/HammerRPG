using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Game.GameTiles.External.Config.Model
{
    public interface IGameItemsConfigService
    {
        void SetItems(IReadOnlyList<IModuleItemConfig> items);
        Optional<IReadOnlyList<IModuleItemConfig>> GetItemsByType(string type);
    }
}