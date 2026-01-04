using System;
using Newtonsoft.Json;

namespace App.Game.Equipment.Runtime.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class EquipmentSlotDto
    {
        [JsonProperty("slot")]
        private string m_Slot;
        
        [JsonProperty("types")]
        private string[] m_Types;

        public string Slot => m_Slot;

        public string[] Types => m_Types;
    }
}