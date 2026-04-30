using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Game.Inventory.External.ViewModel;
using App.Game.Inventory.Runtime.Item;

namespace App.Game.Inventory.External.AddItemStrategy
{
    public class InventoryAddItemStrategy : IInventoryAddItemStrategy
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly InventoryService _service;
        private readonly InventoryWindowController _windowController;

        public InventoryAddItemStrategy(
            IModuleItemsManager moduleItemsManager, 
            InventoryService service, 
            InventoryWindowController windowController)
        {
            _moduleItemsManager = moduleItemsManager;
            _service = service;
            _windowController = windowController;
        }

        public bool AddItem(IModuleItemConfig moduleItemConfig)
        {
            if (moduleItemConfig == null)
            {
                HLogger.LogError("Cannot add null item to inventory");
                return false;
            }

            return AddItem(moduleItemConfig.Id);
        }

        public bool AddItem(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                HLogger.LogError("Cannot add item with null or empty id to inventory");
                return false;
            }
            
            var item = _moduleItemsManager.Create(id);
            if (!item.HasValue)
            {
                HLogger.LogError($"Failed to create item with id {id}");
                return false;
            }
            
            return AddItem(item.Value);
        }

        public bool AddItem(IModuleItem moduleItem)
        {
            var item = _service.AddItem(moduleItem);
            if (!item.HasValue)
            {
                HLogger.LogError($"Failed to add item {moduleItem.Id} to inventory");
                return false;
            }
            
            _windowController.AddItem(item.Value);
            
            return true;
        }
    }
}