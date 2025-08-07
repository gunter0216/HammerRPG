using App.Common.ModuleItem.Runtime.Config.Interfaces;

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
    }
}