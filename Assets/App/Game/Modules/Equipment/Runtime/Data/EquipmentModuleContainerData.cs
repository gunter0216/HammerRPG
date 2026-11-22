using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Equipment.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public sealed class EquipmentModuleContainerData : IContainerData
    {
        public const string ContainerKey = "EquipmentModuleContainerData";

        [JsonProperty("data")] private List<EquipmentModuleData> _data = new();

        IList IContainerData.Data => _data;

        public string GetContainerKey() => ContainerKey;
        public string Name() => nameof(EquipmentModuleContainerData);
    }
}
