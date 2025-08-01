using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IGameItemConfigController
    {
        Optional<IModuleItemConfig> GetConfig(string id);
    }
}