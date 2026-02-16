using System;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SelectBorderingRoomsGenerationConfigDto
    {
        [JsonProperty("minCorridorSize")]
        private int m_MinCorridorSize = 3;

        public int MinCorridorSize => m_MinCorridorSize;
    }
}