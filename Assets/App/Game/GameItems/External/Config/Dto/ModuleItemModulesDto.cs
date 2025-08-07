using App.Game.GameTiles.External.Config.Dto;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace App.Common.ModuleItem.External.Dto
{
    public partial class ModuleItemModulesDto
    {
        [JsonProperty("game_item_type")]
        private GameItemTypeModuleDto m_GameItemType;

        public GameItemTypeModuleDto GameItemType => m_GameItemType;
    }
}