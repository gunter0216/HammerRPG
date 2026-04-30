using App.Common.ModuleItem.Runtime;
using App.Game.Inventory.Runtime.Data.Model;

namespace App.Game.Inventory.Runtime.Item
{
    public class InventoryItem
    {
        private readonly InventoryItemData _data;
        private IModuleItem _moduleItem;

        public InventoryItemData Data => _data;
        public IModuleItem Item
        {
            get => _moduleItem;
            set => _moduleItem = value;
        }

        public InventoryItem(InventoryItemData data)
        {
            _data = data;
        }
        
        public InventoryItem(IModuleItem moduleItem, InventoryItemData data)
        {
            _moduleItem = moduleItem;
            _data = data;
        }
    }
}