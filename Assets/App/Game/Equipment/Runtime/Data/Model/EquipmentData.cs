using System;
using System.Collections.Generic;
using App.Common.Data.Runtime;
using Newtonsoft.Json;

namespace App.Game.Equipment.Runtime.Data.Model
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class EquipmentData : IData
    {
        [JsonProperty("slots")] private List<EquipmentSlotData> m_Slots;

        public List<EquipmentSlotData> Slots
        {
            get => m_Slots;
            set => m_Slots = value;
        }

        public string Name()
        {
            return nameof(EquipmentData);
        }
    }
}