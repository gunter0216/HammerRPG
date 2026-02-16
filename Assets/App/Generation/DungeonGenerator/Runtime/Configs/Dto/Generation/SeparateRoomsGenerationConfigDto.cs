using System;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SeparateRoomsGenerationConfigDto
    {
        [JsonProperty("speed")]
        private int m_Speed = 1;

        public int Speed => m_Speed;
    }
}