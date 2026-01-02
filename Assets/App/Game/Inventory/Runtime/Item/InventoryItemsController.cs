using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Config.Model;
using App.Game.Inventory.Runtime.Data;
using App.Game.Inventory.Runtime.Data.Model;
using App.Game.Inventory.Runtime.Group;

namespace App.Game.Inventory.Runtime.Item
{
    public class InventoryItemsController
    {
        private readonly IInventoryConfigController m_ConfigController;
        private readonly IInventoryDataController m_DataController;
        private readonly IModuleItemsManager m_ModuleItemsManager;
        private readonly InventoryGroupController m_GroupController;
        
        private Dictionary<string, InventoryItem[]> m_ItemsByGroup;
        
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
            CreateMissingDataGroups();
            CorrectGroupsSize();
            CreateGroups();
            
            return true;
        }

        private void CreateGroups()
        {
            var groups = m_DataController.GetGroups();
            m_ItemsByGroup = new Dictionary<string, InventoryItem[]>(groups.Count);
            foreach (var group in groups)
            {
                var array = new InventoryItem[group.Items.Length];
                m_ItemsByGroup[group.Key] = array;

                for (int i = 0; i < group.Items.Length; ++i)
                {
                    var itemData = group.Items[i];
                    if (itemData == null)
                    {
                        continue;
                    } 
                    
                    var moduleItem = m_ModuleItemsManager.Create(itemData.DataReference);
                    if (!moduleItem.HasValue)
                    {
                        HLogger.LogError("Failed to create module item for inventory item: ");
                        continue;
                    }
                    
                    var groupConfig = m_GroupController.GetItemGroup(moduleItem.Value);
                    if (!groupConfig.HasValue)
                    {
                        HLogger.LogError($"Failed to get group for module item: {moduleItem}");
                        continue;
                    }

                    array[i] = new InventoryItem(moduleItem.Value, itemData);
                }
            }
        }

        private void CorrectGroupsSize()
        {
            var cols = m_ConfigController.GetCols();
            var rows = m_ConfigController.GetRows();
            var minArraySize = cols * rows;

            foreach (var groupData in m_DataController.GetGroups())
            {
                if (groupData.Items.Length < minArraySize)
                {
                    var newArray = new InventoryItemData[minArraySize];
                    Array.Copy(groupData.Items, newArray, groupData.Items.Length);
                    groupData.Items = newArray;
                }
            }
        }

        private void CreateMissingDataGroups()
        {
            var cols = m_ConfigController.GetCols();
            var rows = m_ConfigController.GetRows();
            var minArraySize = cols * rows;
            
            var groups = m_ConfigController.GetGroups();
            var dataGroups = m_DataController.GetGroups();
            var missingGroups = new List<string>();
            foreach (var groupConfig in groups)
            {
                if (dataGroups.All(x => x.Key != groupConfig.Id))
                {
                    missingGroups.Add(groupConfig.Id);
                }
            }

            if (missingGroups.Count > 0)
            {
                foreach (var missingGroup in missingGroups)
                {
                    var group = new InventoryGroupData
                    {
                        Key = missingGroup,
                        Items = new InventoryItemData[minArraySize]
                    };

                    m_DataController.AddGroup(group);
                }
            }
        }

        public Optional<IReadOnlyList<InventoryItem>> GetItemsByGroup(IInventoryGroupConfig group)
        {
            if (!m_ItemsByGroup.TryGetValue(group.Id, out var items))
            {
                return Optional<IReadOnlyList<InventoryItem>>.Fail();
            }
            
            return Optional<IReadOnlyList<InventoryItem>>.Success(items);
        }

        public Optional<InventoryItem> AddItem(IModuleItem moduleItem)
        {
            var group = m_GroupController.GetItemGroup(moduleItem);
            if (!group.HasValue)
            {
                HLogger.LogError($"Failed to get group for module item: {moduleItem}");
                return Optional<InventoryItem>.Fail();
            }
            
            if (!m_ItemsByGroup.TryGetValue(group.Value.Id, out var items))
            {
                HLogger.LogError($"Failed to get items matrix for group: {group.Value.Id}");
                return Optional<InventoryItem>.Fail();
            }

            for (int i = 0; i < items.Length; ++i)
            {
                if (items[i] != null)
                {
                    continue;
                }

                var data = new InventoryItemData(i, moduleItem.ReferenceSelf);
                var item = new InventoryItem(moduleItem, data);
                items[i] = item;
                
                m_DataController.SetItem(group.Value.Id, data);
                
                return Optional<InventoryItem>.Success(item);
            }

            HLogger.LogError("not found empty slot");

            return Optional<InventoryItem>.Fail();
        }
        
        public Optional<InventoryItem> AddItem(IModuleItem moduleItem, int index)
        {
            var group = m_GroupController.GetItemGroup(moduleItem);
            if (!group.HasValue)
            {
                HLogger.LogError($"Failed to get group for module item: {moduleItem}");
                return Optional<InventoryItem>.Fail();
            }
            
            if (!m_ItemsByGroup.TryGetValue(group.Value.Id, out var items))
            {
                HLogger.LogError($"Failed to get items matrix for group: {group.Value.Id}");
                return Optional<InventoryItem>.Fail();
            }

            var data = new InventoryItemData(index, moduleItem.ReferenceSelf);
            var item = new InventoryItem(moduleItem, data);
            items[index] = item;
            m_DataController.SetItem(group.Value.Id, data);

            return Optional<InventoryItem>.Success(item);
        }
        
        public void RemoveItem(IModuleItem item, int index)
        {
            var group = m_GroupController.GetItemGroup(item);
            if (!group.HasValue)
            {
                HLogger.LogError($"Failed to get group for module item: {item}");
                return;
            }
            
            if (!m_ItemsByGroup.TryGetValue(group.Value.Id, out var items))
            {
                HLogger.LogError($"Failed to get items matrix for group: {group.Value.Id}");
                return;
            }

            items[index] = null;
            m_DataController.RemoveItem(group.Value.Id, index);
        }
    }
}