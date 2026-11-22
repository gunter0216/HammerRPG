using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using System;
using ItemContainer = App.Game.Containers.Container.Runtime.Container;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime.Config;
using App.Game.Inventory.Runtime.Data;
using App.Game.Inventory.Runtime.Data.Model;

namespace App.Game.Inventory.Runtime.Item
{
    public class InventoryService : IDisposable
    {
        private readonly InventoryConfigService _configService;
        
        private List<InventoryItem> _items;
        private ItemContainer _container;
        public ItemContainer Container => _container;

        public event Action<InventoryItem> ItemChanged;

        public bool BindContainer(ItemContainer container)
        {
            if (container == null || _items == null || container.Items.Count != _items.Count)
                return false;

            if (_container != null)
                _container.ItemChanged -= OnContainerItemChanged;

            _container = container;
            _container.ItemChanged += OnContainerItemChanged;
            for (int i = 0; i < _items.Count; i++)
                OnContainerItemChanged(i);
            return true;
        }

        private void OnContainerItemChanged(int index)
        {
            var item = _container.Items[index];
            _items[index] = new InventoryItem(item, new InventoryItemData(index, item?.ReferenceSelf));
            ItemChanged?.Invoke(_items[index]);
        }

        public void Dispose()
        {
            if (_container != null)
                _container.ItemChanged -= OnContainerItemChanged;
        }
        
        public InventoryService(InventoryConfigService configService)
        {
            _configService = configService;
        }

        public bool Initialize()
        {
            int cols = _configService.GetCols();
            int rows = _configService.GetRows();
            if (cols <= 0 || rows <= 0)
                return false;

            int count = cols * rows;
            _items = new List<InventoryItem>(count);
            for (int i = 0; i < count; i++)
                _items.Add(new InventoryItem(new InventoryItemData(i)));
            
            return true;
        }

        public Optional<InventoryItem> AddItem(IModuleItem moduleItem)
        {
            if (_container == null)
                return Optional<InventoryItem>.Fail();

            int index = _container.FindSpace(moduleItem);
            if (index >= 0)
                return AddItem(moduleItem, index);

            HLogger.LogError("not found empty slot");

            return Optional<InventoryItem>.Fail();
        }
        
        public Optional<InventoryItem> AddItem(IModuleItem moduleItem, int index)
        {
            if (_container == null || moduleItem == null || index < 0 || index >= _items.Count || _items[index].Item != null)
                return Optional<InventoryItem>.Fail();

            if (!_container.AddItem(moduleItem, index))
                return Optional<InventoryItem>.Fail();

            return Optional<InventoryItem>.Success(_items[index]);
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
            if (_container != null && index >= 0 && index < _items.Count)
                _container.Remove(index);
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
