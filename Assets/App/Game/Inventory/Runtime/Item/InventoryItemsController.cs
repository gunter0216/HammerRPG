using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.External.Group;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;
using App.Generation.DungeonGenerator.Runtime.Matrix;

namespace App.Game.Inventory.External
{
    public class InventoryItemsController
    {
        private readonly IInventoryConfigController m_ConfigController;
        private readonly IInventoryDataController m_DataController;
        private readonly IModuleItemsManager m_ModuleItemsManager;
        private readonly InventoryGroupController m_GroupController;
        
        private List<InventoryItem> m_Items;
        private Dictionary<string, Matrix<InventoryItem>> m_ItemsByGroup;
        
        public InventoryItemsController(
            IInventoryConfigController configController, 
            IInventoryDataController dataController, 
            IModuleItemsManager moduleItemsManager, 
            InventoryGroupController groupController)
        {
            m_ConfigController = configController;
            m_DataController = dataController;
            m_ModuleItemsManager = moduleItemsManager;
            m_GroupController = groupController;
        }

        public bool Initialize()
        {
            var groups = m_ConfigController.GetGroups();
            m_ItemsByGroup = new Dictionary<string, Matrix<InventoryItem>>(groups.Count);
            foreach (var group in groups)
            {
                var matrix = new Matrix<InventoryItem>(
                    m_ConfigController.GetCols(),
                    m_ConfigController.GetRows());
                m_ItemsByGroup[group.Id] = matrix;
            }
            
            var items = m_DataController.GetItems();
            m_Items = new List<InventoryItem>(items.Count);
            foreach (var inventoryItemData in items)
            {
                var moduleItem = m_ModuleItemsManager.Create(inventoryItemData.DataReference);
                if (!moduleItem.HasValue)
                {
                    HLogger.LogError("Failed to create module item for inventory item: ");
                    continue;
                }

                AddItem(inventoryItemData, moduleItem.Value);
            }
            
            return true;
        }

        public IReadOnlyList<InventoryItem> GetItems()
        {
            return m_Items;
        }

        public IReadOnlyList<InventoryItem> GetItemsByGroup(IInventoryGroupConfig group)
        {
            var itemsInGroup = new List<InventoryItem>();
            foreach (var item in m_Items)
            {
                if (item.Group.Id == group.Id)
                {
                    itemsInGroup.Add(item);
                }
            }

            return itemsInGroup;
        }

        public Optional<InventoryItem> AddItem(IModuleItem moduleItem)
        {
            var group = m_GroupController.GetItemGroup(moduleItem);
            if (!group.HasValue)
            {
                HLogger.LogError($"Failed to get group for module item: {moduleItem}");
                return Optional<InventoryItem>.Fail();
            }
            
            if (!m_ItemsByGroup.TryGetValue(group.Value.Id, out var itemsMatrix))
            {
                HLogger.LogError($"Failed to get items matrix for group: {group.Value.Id}");
                return Optional<InventoryItem>.Fail();
            }

            var firstEmptyCell = itemsMatrix.GetFirstDefaultCell();
            if (firstEmptyCell == Matrix.InvalidPosition)
            {
                HLogger.LogError("No empty cell found in inventory matrix");
                return Optional<InventoryItem>.Fail();
            }
            
            var inventoryItemData = new InventoryItemData()
            {
                PositionX = firstEmptyCell.Col,
                PositionY = firstEmptyCell.Row,
                DataReference = moduleItem.ReferenceSelf
            };
            
            var item = CreateItem(inventoryItemData, moduleItem, group.Value);
            AddItem(item, itemsMatrix);
            AddItemInData(item);

            return Optional<InventoryItem>.Success(item);
        }

        // public Optional<InventoryItem> AddItem(IModuleItem moduleItem, int positionX, int positionY)
        // {
        //     var inventoryItemData = new InventoryItemData()
        //     {
        //         PositionX = positionX,
        //         PositionY = positionY,
        //         DataReference = moduleItem.ReferenceSelf
        //     };
        //     
        //     return AddItem(inventoryItemData, moduleItem);
        // }
        //
        private Optional<InventoryItem> AddItem(InventoryItemData inventoryItemData, IModuleItem moduleItem)
        {
            var group = m_GroupController.GetItemGroup(moduleItem);
            if (!group.HasValue)
            {
                HLogger.LogError($"Failed to get group for module item: {moduleItem}");
                return Optional<InventoryItem>.Fail();
            }
            
            if (!m_ItemsByGroup.TryGetValue(group.Value.Id, out var itemsMatrix))
            {
                HLogger.LogError($"Failed to get items matrix for group: {group.Value.Id}");
                return Optional<InventoryItem>.Fail();
            }
            
            var item = new InventoryItem(inventoryItemData, moduleItem, group.Value);
            AddItem(item, itemsMatrix);
            
            return Optional<InventoryItem>.Success(item);
        }

        private void AddItem(InventoryItem inventoryItem, Matrix<InventoryItem> itemsMatrix)
        {
            m_Items.Add(inventoryItem);
            
            itemsMatrix.SetCell(
                inventoryItem.Data.PositionY, 
                inventoryItem.Data.PositionX, 
                inventoryItem);
        }

        private void AddItemInData(InventoryItem inventoryItem)
        {
            m_DataController.AddItem(inventoryItem.Data);
        }

        private InventoryItem CreateItem(
            InventoryItemData inventoryItemData, 
            IModuleItem moduleItem, 
            IInventoryGroupConfig group)
        {
            return new InventoryItem(inventoryItemData, moduleItem, group);
        }
    }
}