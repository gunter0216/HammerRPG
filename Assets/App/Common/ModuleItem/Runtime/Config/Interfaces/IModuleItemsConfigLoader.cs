using App.Common.ModuleItem.Runtime.Config.Dto;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IModuleItemsConfigLoader
    {
        Optional<ModuleItemsDto> Load();
    }
}