using App.Game.GameTiles.External.Config.Dto;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace App.Common.ModuleItem.External.Dto
{
    public partial class ModuleItemModulesDto
    {
        [JsonProperty("icon")]
        private SpriteModuleDto m_SpriteModule;

        public SpriteModuleDto SpriteModule => m_SpriteModule;
    }
}