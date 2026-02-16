using System;
using App.Generation.DungeonGenerator.External.Dto.Common;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SelectSmallRoomsGenerationConfigDto
    {
        [JsonProperty("roomThreshold")]
        private SizeIntDto m_RoomThreshold;

        public SizeIntDto RoomThreshold => m_RoomThreshold;
    }
}
