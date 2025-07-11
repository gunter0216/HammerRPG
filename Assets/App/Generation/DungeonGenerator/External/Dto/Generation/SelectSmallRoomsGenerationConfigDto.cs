using System;
using App.Generation.DungeonGenerator.External.Dto.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SelectSmallRoomsGenerationConfigDto
    {
        [SerializeField, JsonProperty("Room Threshold")]
        private SizeIntDto m_RoomThreshold;

        public SizeIntDto RoomThreshold => m_RoomThreshold;
    }
}
