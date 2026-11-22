using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.ContainerItem.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ContainerItemModuleData : IModuleData
    {
        [JsonProperty("index")] private int _index = -1;

        public int Index
        {
            get => _index;
            set => _index = value;
        }

        public string GetModuleKey() => ContainerItemContainerData.ContainerKey;
    }
}
