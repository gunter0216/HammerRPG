using System;
using App.Generation.DungeonGenerator.External.Dto;
using Newtonsoft.Json;

namespace App.Game.GameManagers.External.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GenerationConfigDto
    {
        [JsonProperty("generation")] 
        private DungeonGenerationConfigDto m_Generation;

        public DungeonGenerationConfigDto Generation => m_Generation;
    }
}