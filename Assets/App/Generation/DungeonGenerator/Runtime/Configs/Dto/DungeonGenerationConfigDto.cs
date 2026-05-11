using System;
using App.Generation.DungeonGenerator.External.Dto.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DungeonGenerationConfigDto
    {
        [JsonProperty("key")] [SerializeField]
        private string m_Key;
        
        [JsonProperty("rooms")] [SerializeField]
        private CreateRoomsGenerationConfigDto m_RoomsGeneration;

        [JsonProperty("separation")] [SerializeField]
        private SeparateRoomsGenerationConfigDto m_SeparationConfig;

        [JsonProperty("smallRooms")] [SerializeField]
        private SelectSmallRoomsGenerationConfigDto m_SmallRooms;

        [JsonProperty("borderingRooms")] [SerializeField]
        private SelectBorderingRoomsGenerationConfigDto m_BorderingRooms;
        
        [JsonProperty("squareGeneration")] [SerializeField]
        private SquareGenerationConfigDto _squareGeneration;
        
        [JsonProperty("tileBased")] [SerializeField]
        private TileBasedGenerationConfig _tileTileBasedGenerationConfigDto;

        public CreateRoomsGenerationConfigDto RoomsGeneration => m_RoomsGeneration;

        public SeparateRoomsGenerationConfigDto SeparationConfig => m_SeparationConfig;

        public SelectSmallRoomsGenerationConfigDto SmallRooms => m_SmallRooms;

        public SelectBorderingRoomsGenerationConfigDto BorderingRooms => m_BorderingRooms;

        public string Key => m_Key;

        public SquareGenerationConfigDto SquareGeneration => _squareGeneration;
        public TileBasedGenerationConfig TileBasedGenerationConfigDto => _tileTileBasedGenerationConfigDto;
    }
}