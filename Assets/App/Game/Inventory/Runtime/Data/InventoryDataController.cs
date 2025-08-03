using App.Game.Inventory.External.Data;

namespace App.Game.Inventory.Runtime.Data
{
    public class InventoryDataController
    {
        private readonly InventoryData m_Data;
        
        public InventoryDataController(InventoryData data)
        {
            m_Data = data;
        }
    }
}