using System.Collections.Generic;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;
using Assets.App.Common.ModuleItem.Runtime;

namespace App.Game.Inventory.External
{
    public class InventoryItemsController
    {
        private readonly IInventoryConfigController m_ConfigController;
        private readonly IInventoryDataController m_DataController;
        private readonly IModuleItemsManager m_ModuleItemsManager;
        
        private List<InventoryItem> m_Items;
        
        public InventoryItemsController(
            IInventoryConfigController configController, 
            IInventoryDataController dataController, 
            IModuleItemsManager moduleItemsManager)
        {
            m_ConfigController = configController;
            m_DataController = dataController;
            m_ModuleItemsManager = moduleItemsManager;
        }

        public bool Initialize()
        {
            var items = m_DataController.GetItems();
            m_Items = new List<InventoryItem>(items.Count);
            // foreach (var itemData in items)
            // {
            //     var moduleItem = m_ModuleItemsManager.Create(itemData.ModuleItemId);
            //     if (moduleItem == null)
            //     {
            //         continue;
            //     }
            //
            //     var item = new InventoryItem(itemData, moduleItem);
            //     m_Items.Add(item);
            // }
            return true;
        }
    }
}