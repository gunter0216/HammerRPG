using System;
using App.Generation.DungeonGenerator.External.Dto.Generation;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DungeonGenerationConfigDto
    {
        [JsonProperty("key")]
        private string m_Key;
        
        [JsonProperty("rooms")]
        private CreateRoomsGenerationConfigDto m_RoomsGeneration;

        [JsonProperty("separation")]
        private SeparateRoomsGenerationConfigDto m_SeparationConfig;

        [JsonProperty("smallRooms")]
        private SelectSmallRoomsGenerationConfigDto m_SmallRooms;

        [JsonProperty("borderingRooms")]
        private SelectBorderingRoomsGenerationConfigDto m_BorderingRooms;

        public CreateRoomsGenerationConfigDto RoomsGeneration => m_RoomsGeneration;

        public SeparateRoomsGenerationConfigDto SeparationConfig => m_SeparationConfig;

        public SelectSmallRoomsGenerationConfigDto SmallRooms => m_SmallRooms;

        public SelectBorderingRoomsGenerationConfigDto BorderingRooms => m_BorderingRooms;

        public string Key => m_Key;
    }
}