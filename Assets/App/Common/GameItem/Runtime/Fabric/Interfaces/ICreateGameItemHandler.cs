using App.Common.Utility.Runtime;

namespace App.Common.GameItem.Runtime.Fabric.Interfaces
{
    public interface ICreateGameItemHandler
    {
        Optional<IGameItem> Handle(IGameItem gameItem);
    }
}