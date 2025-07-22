using App.Common.GameItem.Runtime;
using App.Common.GameItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.External
{
    public interface IGameItemsManager
    {
        Optional<IGameItem> Create(string id);
        Optional<IGameItem> Create(IGameItemData data);
        bool Destroy(IGameItemData data);
    }
}