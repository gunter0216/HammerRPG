using System;
using Newtonsoft.Json;

namespace App.Game.Equipment.Runtime.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class EquipmentDto
    {
        [JsonProperty("slots")]
        private EquipmentSlotDto[] m_Slots;

        public EquipmentSlotDto[] Slots => m_Slots;
    }
}
