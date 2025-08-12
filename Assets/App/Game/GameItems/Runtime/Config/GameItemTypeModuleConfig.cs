using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Game.GameTiles.External.Config.Dto;

namespace App.Game.GameTiles.External.Config.Model
{
    public class GameItemTypeModuleConfig : IModuleConfig
    {
        private readonly string m_Type;

        public string Type => m_Type;

        public GameItemTypeModuleConfig(string type)
        {
            m_Type = type;
        }

        public GameItemTypeModuleConfig(GameItemTypeModuleDto dto)
        {
            m_Type = dto.Type;
        }
    }
}