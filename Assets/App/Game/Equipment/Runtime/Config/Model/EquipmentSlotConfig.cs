using System.Collections.Generic;
using App.Game.Equipment.Runtime.Config.Dto;

namespace App.Game.Equipment.Runtime.Config.Model
{
    public class EquipmentSlotConfig
    {
        private readonly string m_Slot;
        private readonly string[] m_Types;
        
        public string Slot => m_Slot;
        public IReadOnlyList<string> Types => m_Types;

        public EquipmentSlotConfig(EquipmentSlotDto dto)
        {
            m_Slot = dto.Slot;
            m_Types = dto.Types;
        }
    }
}