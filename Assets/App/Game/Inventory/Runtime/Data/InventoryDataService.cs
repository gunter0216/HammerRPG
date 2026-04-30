using System.Collections.Generic;
using System.Linq;
using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Game.Inventory.Runtime.Data.Loader;
using App.Game.Inventory.Runtime.Data.Model;

namespace App.Game.Inventory.Runtime.Data
{
    public class InventoryDataService : IInventoryDataService
    {
        private readonly IDataManager _dataManager;
        
        private InventoryData _data;
        private int _rows;
        private int _cold;

        public InventoryDataService(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public bool Initialize(int cold, int rows)
        {
            _rows = rows;
            _cold = cold;
            
            var dataLoader = new InventoryDataLoader(_dataManager);
            var data = dataLoader.Load();
            if (!data.HasValue)
            {
                HLogger.LogError("InventoryData is null");
                return false;
            }
            
            _data = data.Value;

            _data.Items ??= new List<InventoryItemData>();
            for (int i = _data.Items.Count; i < rows * cold; ++i)
            {
                _data.Items.Add(new InventoryItemData(i));
            }
            
            return true;
        }

        public IReadOnlyList<InventoryItemData> GetItems()
        {
            return _data.Items;
        }

        public bool RemoveItem(int index)
        {
            if (index < 0 || index >= _data.Items.Count)
            {
                HLogger.LogError("Index is not correct.");
                return false;
            }

            _data.Items[index].DataReference = null;
            
            return true;
        }

        public void AddItem(InventoryItemData data)
        {
            _data.Items[data.Index] = data;
        }
    }
}