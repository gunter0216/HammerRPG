using App.Common.ModuleItem.Runtime;
using App.Game.Inventory.Runtime.Data.Model;

namespace App.Game.Inventory.Runtime.Item
{
    public class InventoryItem
    {
        private readonly InventoryItemData m_Data;
        private readonly IModuleItem m_ModuleItem;

        public InventoryItemData Data => m_Data;
        public IModuleItem Item => m_ModuleItem;

        // public InventoryItem(InventoryItemData data, IModuleItem moduleItem, IInventoryGroupConfig group)
        // {
        //     // m_Data = data;
        //     m_ModuleItem = moduleItem;
        //     // m_Group = group;
        // }
        
        public InventoryItem(IModuleItem moduleItem, InventoryItemData data)
        {
            m_ModuleItem = moduleItem;
            m_Data = data;
        }
    }
}