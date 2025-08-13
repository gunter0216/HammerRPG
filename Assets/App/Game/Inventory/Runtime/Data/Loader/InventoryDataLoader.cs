using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime.Data;

namespace App.Game.Inventory.External.Data
{
    public class InventoryDataLoader
    {
        private readonly IDataManager m_DataManager;

        public InventoryDataLoader(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public Optional<InventoryData> Load()
        {
            var data = m_DataManager.GetData<InventoryData>(nameof(InventoryData));
            if (!data.HasValue)
            {
                HLogger.LogError("InventoryData is null");
                return Optional<InventoryData>.Fail();
            }
            
            return data;
        }
    }
}