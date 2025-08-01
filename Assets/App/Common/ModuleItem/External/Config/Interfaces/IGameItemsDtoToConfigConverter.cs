using App.Common.ModuleItem.External.Dto;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.External.Config.Interfaces
{
    public interface IGameItemsDtoToConfigConverter
    {
        Optional<IGameItemsConfig> Convert(GameItemsDto dto);
    }
}