using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime;

namespace App.Game.GameItems.Runtime
{
    public interface IGameItemsManager
    {
        ModuleItemResult<IGameModuleItem> Create(DataReference dataReference);
        ModuleItemResult<IGameModuleItem> Create(string id);
        bool Destroy(IGameModuleItem data);
    }
}