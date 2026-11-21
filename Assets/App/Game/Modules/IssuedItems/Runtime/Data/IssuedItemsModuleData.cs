using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.IssuedItems.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class IssuedItemsModuleData : IModuleData
    {
        [JsonProperty("issuedCount")] private int _issuedCount;

        public int IssuedCount
        {
            get => _issuedCount;
            internal set => _issuedCount = value;
        }

        public string GetModuleKey() => IssuedItemsContainerData.ContainerKey;
    }
}
