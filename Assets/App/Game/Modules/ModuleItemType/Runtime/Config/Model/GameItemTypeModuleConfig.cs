using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model
{
    public class GameItemTypeModuleConfig : ModuleConfig
    {
        private readonly string m_Type;

        public string Type => m_Type;

        public GameItemTypeModuleConfig(string type)
        {
            m_Type = type;
        }
    }
}