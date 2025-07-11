using System;
using App.Generation.DungeonGenerator.External.Dto.Generation;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DungeonGenerationConfigDto
    {
        [SerializeField, JsonProperty("Rooms")]
        private CreateRoomsGenerationConfigDto m_RoomsGeneration;

        [SerializeField, JsonProperty("Separation")]
        private SeparateRoomsGenerationConfigDto m_SeparationConfig;

        [SerializeField, JsonProperty("Small rooms")]
        private SelectSmallRoomsGenerationConfigDto m_SmallRooms;

        [SerializeField, JsonProperty("Bordering rooms")]
        private SelectBorderingRoomsGenerationConfigDto m_BorderingRooms;

        public CreateRoomsGenerationConfigDto RoomsGeneration => m_RoomsGeneration;

        public SeparateRoomsGenerationConfigDto SeparationConfig => m_SeparationConfig;

        public SelectSmallRoomsGenerationConfigDto SmallRooms => m_SmallRooms;

        public SelectBorderingRoomsGenerationConfigDto BorderingRooms => m_BorderingRooms;
    }
}