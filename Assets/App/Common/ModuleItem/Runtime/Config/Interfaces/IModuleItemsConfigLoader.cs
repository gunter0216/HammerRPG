using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Config.Dto;

namespace Assets.App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IModuleItemsConfigLoader
    {
        Optional<ModuleItemsDto> Load();
    }
}