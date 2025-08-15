using App.Common.ModuleItem.Runtime;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;

namespace App.Game.Inventory.External
{
    public class InventoryItem
    {
        private readonly InventoryItemData m_Data;
        private readonly IModuleItem m_ModuleItem;
        private readonly IInventoryGroupConfig m_Group;

        public InventoryItemData Data => m_Data;
        public IModuleItem Item => m_ModuleItem;
        public IInventoryGroupConfig Group => m_Group;

        public InventoryItem(InventoryItemData data, IModuleItem moduleItem, IInventoryGroupConfig group)
        {
            m_Data = data;
            m_ModuleItem = moduleItem;
            m_Group = group;
        }
    }
}