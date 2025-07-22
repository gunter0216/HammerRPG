using App.Common.GameItem.Runtime.Data;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.Runtime.Fabric.Interfaces
{
    public interface IGameItemCreator
    {
        Optional<IGameItem> Create(string id);
        Optional<IGameItem> Create(IGameItemData data);
    }
}