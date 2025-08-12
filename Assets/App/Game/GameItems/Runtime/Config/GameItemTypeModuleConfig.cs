using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Game.GameItems.Runtime.Config.Dto;

namespace App.Game.GameItems.Runtime.Config
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