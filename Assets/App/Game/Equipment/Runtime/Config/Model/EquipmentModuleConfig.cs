using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.ModuleItemType.Runtime.Config.Model
{
    public class EquipmentModuleConfig : IModuleConfig
    {
        private readonly string m_Type;

        public string Type => m_Type;

        public EquipmentModuleConfig(string type)
        {
            m_Type = type;
        }
    }
}