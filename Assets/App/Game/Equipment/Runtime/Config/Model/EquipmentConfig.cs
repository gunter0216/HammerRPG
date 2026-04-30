using System.Collections.Generic;
using App.Game.Equipment.Runtime.Config.Dto;

namespace App.Game.Equipment.Runtime.Config.Model
{
    public class EquipmentConfig
    {
        private readonly EquipmentSlotConfig[] m_Slots;

        public IReadOnlyList<EquipmentSlotConfig> Slots => m_Slots;
        
        public EquipmentConfig(EquipmentDto dto)
        {
            m_Slots = new EquipmentSlotConfig[dto.Slots.Length];
            for (int i = 0; i < dto.Slots.Length; ++i)
            {
                m_Slots[i] = new EquipmentSlotConfig(dto.Slots[i]);
            }
        }
    }
}
