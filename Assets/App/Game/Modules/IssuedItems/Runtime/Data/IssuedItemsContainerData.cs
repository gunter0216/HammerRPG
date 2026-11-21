using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.IssuedItems.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class IssuedItemsContainerData : IContainerData
    {
        public static string ContainerKey => "IssuedItemsContainerData";

        [JsonProperty("data")] private List<IssuedItemsModuleData> _data = new();

        IList IContainerData.Data => _data;

        public string GetContainerKey() => ContainerKey;
        public string Name() => nameof(IssuedItemsContainerData);
    }
}
