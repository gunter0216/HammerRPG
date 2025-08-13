using App.Game.Inventory.Runtime.Data;
using Assets.App.Common.ModuleItem.Runtime;

namespace App.Game.Inventory.External
{
    public class InventoryItem
    {
        private readonly InventoryItemData m_Data;
        private readonly IModuleItem m_ModuleItem;

        public InventoryItemData Data => m_Data;

        public IModuleItem Item => m_ModuleItem;

        public InventoryItem(InventoryItemData data, IModuleItem moduleItem)
        {
            m_Data = data;
            m_ModuleItem = moduleItem;
        }
    }
}