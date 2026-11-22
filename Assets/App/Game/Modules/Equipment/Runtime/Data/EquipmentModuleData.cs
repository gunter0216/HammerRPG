using System;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Data;
using App.Game.Modules.Equipment.Runtime.Config;
using Newtonsoft.Json;

namespace App.Game.Modules.Equipment.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public sealed class EquipmentModuleData : IModuleData
    {
        [JsonProperty("slots")]
        private Dictionary<EquipmentSlotId, DataReference> _slots = new();

        internal Dictionary<EquipmentSlotId, DataReference> Slots => _slots ??= new();

        public string GetModuleKey() => EquipmentModuleContainerData.ContainerKey;
    }
}
