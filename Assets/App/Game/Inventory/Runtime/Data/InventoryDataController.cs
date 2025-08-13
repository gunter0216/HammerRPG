using System.Collections.Generic;
using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Game.Inventory.External.Data;

namespace App.Game.Inventory.Runtime.Data
{
    public class InventoryDataController : IInventoryDataController
    {
        private readonly IDataManager m_DataManager;
        
        private InventoryData m_Data;
        
        public InventoryDataController(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public bool Initialize()
        {
            var dataLoader = new InventoryDataLoader(m_DataManager);
            var data = dataLoader.Load();
            if (!data.HasValue)
            {
                HLogger.LogError("InventoryData is null");
                return false;
            }
            
            m_Data = data.Value;
            
            m_Data.Items ??= new List<InventoryItemData>();
            
            return true;
        }


        public List<InventoryItemData> GetItems()
        {
            return m_Data.Items;
        }
    }
}