using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;
using App.Game.Inventory.Runtime.Data.Model;

namespace App.Game.Inventory.Runtime.Item
{
    public class InventoryService
    {
        private readonly InventoryConfigService _configService;
        private readonly InventoryDataService _dataService;
        private readonly IModuleItemsManager _moduleItemsManager;
        
        private List<InventoryItem> _items;
        
        public InventoryService(
            InventoryConfigService configService, 
            InventoryDataService dataService, 
            IModuleItemsManager moduleItemsManager)
        {
            _configService = configService;
            _dataService = dataService;
            _moduleItemsManager = moduleItemsManager;
        }

        public bool Initialize()
        {
            var items = _dataService.GetItems();
            _items = new List<InventoryItem>(items.Count);
            foreach (var itemData in items)
            {
                var item = new InventoryItem(itemData);

                if (itemData.DataReference != null)
                {
                    var moduleItem = _moduleItemsManager.Create(itemData.DataReference);
                    if (moduleItem.HasValue)
                    {
                        item.Item = moduleItem.Value;
                    }
                }

                _items.Add(item);
            }
            
            return true;
        }

        public Optional<InventoryItem> AddItem(IModuleItem moduleItem)
        {
            for (int i = 0; i < _items.Count; ++i)
            {
                if (_items[i].Item != null)
                {
                    continue;
                }

                return AddItem(moduleItem, i);
            }

            HLogger.LogError("not found empty slot");

            return Optional<InventoryItem>.Fail();
        }
        
        public Optional<InventoryItem> AddItem(IModuleItem moduleItem, int index)
        {
            var data = new InventoryItemData(index, moduleItem.ReferenceSelf);
            var item = new InventoryItem(moduleItem, data);
                
            _items[index] = item;
            _dataService.AddItem(data);
            
            return Optional<InventoryItem>.Success(item);
        }

        public IReadOnlyList<InventoryItem> GetItems()
        {
            return _items;
        }

        public void Remove(InventoryItem inventoryItem)
        {
            RemoveItem(inventoryItem.Data.Index);
        }

        public void RemoveItem(int index)
        {
            _items[index].Item = null;
            _dataService.RemoveItem(index);
        }

        public int GetRows()
        {
            return _configService.GetRows();
        }

        public int GetCols()
        {
            return _configService.GetCols();
        }

        public bool TryGetItem(DataReference dataReference, out InventoryItem item)
        {
            foreach (var inventoryItem in _items)
            {
                if (inventoryItem.Item == null)
                {
                    continue;
                }

                if (inventoryItem.Item.ReferenceSelf == dataReference)
                {
                    item = inventoryItem;
                    return true;
                }
            }

            item = null;
            
            return false;
        }
    }
}