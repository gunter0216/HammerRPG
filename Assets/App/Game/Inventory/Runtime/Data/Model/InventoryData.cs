using System;
using System.Collections.Generic;
using App.Common.Data.Runtime;
using Newtonsoft.Json;

namespace App.Game.Inventory.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryData : IData
    {
        [JsonProperty("groups")] private List<InventoryGroupData> m_Groups;

        public List<InventoryGroupData> Groups
        {
            get => m_Groups;
            set => m_Groups = value;
        }

        public string Name()
        {
            return nameof(InventoryData);
        }
    }
}