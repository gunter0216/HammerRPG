using App.Common.ModuleItem.Runtime;
using App.Game.Equipment.Runtime.Data.Model;

namespace App.Game.Equipment.Runtime.Item
{
    public class EquipmentSlot
    {
        private readonly EquipmentSlotData m_Data;
        private IModuleItem m_ModuleItem;

        public EquipmentSlotData Data => m_Data;
        public IModuleItem Item
        {
            get => m_ModuleItem;
            set => m_ModuleItem = value;
        }

        public EquipmentSlot(EquipmentSlotData data)
        {
            m_Data = data;
        }
    }
}