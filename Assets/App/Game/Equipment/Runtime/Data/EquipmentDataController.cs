using System.Collections.Generic;
using System.Linq;
using App.Common.Data.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Game.Equipment.External.Data;

namespace App.Game.Equipment.Runtime.Data
{
    public class EquipmentDataController : IEquipmentDataController
    {
        private readonly IDataManager m_DataManager;
        
        private EquipmentData m_Data;
        
        public EquipmentDataController(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public bool Initialize()
        {
            var dataLoader = new EquipmentDataLoader(m_DataManager);
            var data = dataLoader.Load();
            if (!data.HasValue)
            {
                HLogger.LogError("EquipmentData is null");
                return false;
            }
            
            m_Data = data.Value;
            
            m_Data.Slots ??= new List<EquipmentSlotData>();
            AddSlot(EquipmentSlotConstants.Amulet);
            
            return true;
        }

        private void AddSlot(string slot)
        {
            if (m_Data.Slots.Any(x => x.SlotKey == slot))
            {
                return;
            }
            
            AddSlot(new EquipmentSlotData(slot, null));
        }

        public IReadOnlyList<EquipmentSlotData> GetSlots()
        {
            return m_Data.Slots;
        }

        public bool AddSlot(EquipmentSlotData slotData)
        {
            m_Data.Slots.Add(slotData);
            return true;
        }

        public bool RemoveItem(string slot)
        {
            var slotData = m_Data.Slots.FirstOrDefault(x => x.SlotKey == slot);
            if (slotData == default)
            {
                HLogger.LogError("Slot not found");
                return false;
            }

            slotData.DataReference = null;
            
            return true;
        }

        public void SetItem(string slot, DataReference dataReference)
        {
            var slotData = m_Data.Slots.FirstOrDefault(x => x.SlotKey == slot);
            if (slotData == default)
            {
                HLogger.LogError("Slot not found");
                return;
            }

            slotData.DataReference = dataReference;
        }
    }
}