using System;

namespace App.Game.Inventory.Runtime.Config
{
    public class InventoryGroupConfig : IInventoryGroupConfig
    {
        private readonly string m_Id;
        private readonly string m_Icon;

        public InventoryGroupConfig(string id, string icon)
        {
            m_Id = id;
            m_Icon = icon;
        }

        public string Id => m_Id;
        public string Icon => m_Icon;
    }
}
