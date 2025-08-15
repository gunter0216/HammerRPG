using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Game.GameTiles.External.Config.Dto;

namespace App.Game.GameTiles.External.Config.Model
{
    public class SpriteModuleConfig : IModuleConfig
    {
        private readonly string m_Key;

        public string Key => m_Key;

        public SpriteModuleConfig(string key)
        {
            m_Key = key;
        }

        public SpriteModuleConfig(SpriteModuleDto dto)
        {
            m_Key = dto.Key;
        }
    }
}