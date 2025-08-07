using System;

namespace App.Game.Inventory.Runtime.Config
{
    public class InventoryGroupConfig : IInventoryGroupConfig
    {
        private readonly string m_Id;
        private readonly string m_Icon;
        private readonly string m_GameType;

        public InventoryGroupConfig(string id, string icon, string gameType)
        {
            m_Id = id;
            m_Icon = icon;
            m_GameType = gameType;
        }

        public string Id => m_Id;
        public string Icon => m_Icon;

        public string GameType => m_GameType;
    }
}
