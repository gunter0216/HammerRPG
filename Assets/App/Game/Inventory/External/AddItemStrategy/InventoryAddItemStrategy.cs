using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Game.Inventory.External.ViewModel;

namespace App.Game.Inventory.External.AddItemStrategy
{
    public class InventoryAddItemStrategy : IInventoryAddItemStrategy
    {
        private readonly IModuleItemsManager m_ModuleItemsManager;
        private readonly InventoryItemsController m_ItemsController;
        private readonly InventoryWindowModel m_WindowModel;

        public InventoryAddItemStrategy(
            IModuleItemsManager moduleItemsManager, 
            InventoryItemsController itemsController, 
            InventoryWindowModel windowModel)
        {
            m_ModuleItemsManager = moduleItemsManager;
            m_ItemsController = itemsController;
            m_WindowModel = windowModel;
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
            
            var item = m_ModuleItemsManager.Create(id);
            if (!item.HasValue)
            {
                HLogger.LogError($"Failed to create item with id {id}");
                return false;
            }
            
            HLogger.LogError($">>> Created item {item.Value.ReferenceSelf} for inventory");
            
            return AddItem(item.Value);
        }

        public bool AddItem(IModuleItem moduleItem)
        {
            var item = m_ItemsController.AddItem(moduleItem);
            if (!item.HasValue)
            {
                HLogger.LogError($"Failed to add item {moduleItem.Id} to inventory");
                return false;
            }
            
            m_WindowModel.AddItem(item.Value);
            
            return true;
        }
    }
}