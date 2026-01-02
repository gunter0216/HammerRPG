using System.Collections.Generic;
using System.Linq;
using App.Common.Data.Runtime;
using App.Common.DataContainer.Runtime;
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
            
            m_Data.Groups ??= new List<InventoryGroupData>();
            
            return true;
        }

        public IReadOnlyList<InventoryGroupData> GetGroups()
        {
            return m_Data.Groups;
        }

        public bool AddGroup(InventoryGroupData groupData)
        {
            m_Data.Groups.Add(groupData);
            return true;
        }

        public bool RemoveItem(string group, int index)
        {
            var groupData = m_Data.Groups.FirstOrDefault(x => x.Key == group);
            if (groupData == default)
            {
                HLogger.LogError("group not found");
                return false;
            }

            groupData.Items[index] = null;
            
            return true;
        }

        // public bool AddItem(InventoryItemData itemData)
        // {
        //     // 2 3
        //     // б х 
        //     //
        //     //
        //     // 
        //     // m_Data.Groups.Add(itemData);
        //     return true;
        // }

        public void SetItem(string group, InventoryItemData data)
        {
            var groupData = m_Data.Groups.FirstOrDefault(x => x.Key == group);
            if (groupData == default)
            {
                HLogger.LogError("group not found");
                return;
            }

            groupData.Items[data.Index] = data;
        }
    }
}