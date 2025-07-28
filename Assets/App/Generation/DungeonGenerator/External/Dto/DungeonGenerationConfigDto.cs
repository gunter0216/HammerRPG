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
        [SerializeField, JsonProperty("rooms")]
        private CreateRoomsGenerationConfigDto m_RoomsGeneration;

        [SerializeField, JsonProperty("separation")]
        private SeparateRoomsGenerationConfigDto m_SeparationConfig;

        [SerializeField, JsonProperty("smallRooms")]
        private SelectSmallRoomsGenerationConfigDto m_SmallRooms;

        [SerializeField, JsonProperty("borderingRooms")]
        private SelectBorderingRoomsGenerationConfigDto m_BorderingRooms;

        public CreateRoomsGenerationConfigDto RoomsGeneration => m_RoomsGeneration;

        public SeparateRoomsGenerationConfigDto SeparationConfig => m_SeparationConfig;

        public SelectSmallRoomsGenerationConfigDto SmallRooms => m_SmallRooms;

        public SelectBorderingRoomsGenerationConfigDto BorderingRooms => m_BorderingRooms;
    }
}