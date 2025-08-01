using App.Common.ModuleItem.External.Dto;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.External.Config.Interfaces
{
    public interface IGameItemsConfigLoader
    {
        Optional<GameItemsDto> Load();
    }
}