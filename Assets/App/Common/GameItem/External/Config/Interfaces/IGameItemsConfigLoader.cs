using App.Common.GameItem.External.Dto;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.External.Config.Interfaces
{
    public interface IGameItemsConfigLoader
    {
        Optional<GameItemsDto> Load();
    }
}