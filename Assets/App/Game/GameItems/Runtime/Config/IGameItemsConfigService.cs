using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace Assets.App.Game.GameItems.Runtime.Config
{
    public interface IGameItemsConfigService
    {
        void SetItems(IReadOnlyList<IModuleItemConfig> items);
        Optional<IReadOnlyList<IModuleItemConfig>> GetItemsByType(string type);
    }
}