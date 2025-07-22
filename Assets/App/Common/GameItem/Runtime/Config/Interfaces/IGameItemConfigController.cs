using App.Common.Utility.Runtime;

namespace App.Common.GameItem.Runtime.Config.Interfaces
{
    public interface IGameItemConfigController
    {
        Optional<IGameItemConfig> GetConfig(string id);
    }
}