using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Equipment.Runtime.Config;
using App.Game.Equipment.Runtime.Data;
using App.Game.Equipment.Runtime.Item;

namespace App.Game.Equipment.Runtime.Items
{
    public class EquipmentSlotsController
    {
        private readonly EquipmentConfigController m_ConfigController;
        private readonly IEquipmentDataController m_DataController;
        private readonly IModuleItemsManager m_ModuleItemsManager;

        private List<EquipmentSlot> m_Slots;
        
        public EquipmentSlotsController(
            EquipmentConfigController configController, 
            IEquipmentDataController dataController, 
            IModuleItemsManager moduleItemsManager)
        {
            m_ConfigController = configController;
            m_DataController = dataController;
            m_ModuleItemsManager = moduleItemsManager;
        }

        public IReadOnlyList<EquipmentSlot> GetSlots()
        {
            return m_Slots;
        }

        public bool Initialize()
        {
            var dataSlots = m_DataController.GetSlots();
            m_Slots = new List<EquipmentSlot>(dataSlots.Count);
            foreach (var dataSlot in dataSlots)
            {
                var item = new EquipmentSlot(dataSlot);
                m_Slots.Add(item);
                
                if (dataSlot.DataReference == null)
                {
                    continue;
                }
                else
                {
                    var moduleItem = m_ModuleItemsManager.Create(dataSlot.DataReference);
                    if (!moduleItem.HasValue)
                    {
                        HLogger.LogError("Failed to create module item for equipment item: ");
                        continue;
                    }

                    item.Item = moduleItem.Value;
                }
            }
            
            return true;
        }

        public void RemoveItem(EquipmentSlot slot)
        {
            slot.Item = null;
            slot.Data.DataReference = null;
            // todo
        }

        public void AddItem(EquipmentSlot slot, IModuleItem item)
        {
            slot.Item = item;
            slot.Data.DataReference = item.ReferenceSelf;
        }
    }
}