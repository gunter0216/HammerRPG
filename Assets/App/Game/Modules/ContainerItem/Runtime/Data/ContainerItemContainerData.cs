using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.ContainerItem.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ContainerItemContainerData : IContainerData
    {
        public static string ContainerKey => "ContainerItemContainerData";

        [JsonProperty("data")] private List<ContainerItemModuleData> _data = new();

        IList IContainerData.Data => _data;

        public string GetContainerKey() => ContainerKey;
        public string Name() => nameof(ContainerItemContainerData);
    }
}
