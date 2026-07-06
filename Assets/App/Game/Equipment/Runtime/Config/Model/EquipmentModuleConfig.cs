using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Equipment.Runtime.Config.Model
{
    public class EquipmentModuleConfig : ModuleConfig
    {
        private readonly string m_Type;

        public string Type => m_Type;

        public EquipmentModuleConfig(string type)
        {
            m_Type = type;
        }
    }
}