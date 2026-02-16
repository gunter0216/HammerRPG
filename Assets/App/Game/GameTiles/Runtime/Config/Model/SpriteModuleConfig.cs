using App.Common.ModuleItem.Runtime.Config.Interfaces;

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
    }
}