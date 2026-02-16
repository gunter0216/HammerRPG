using System;
using App.Generation.DungeonGenerator.External.Dto;
using Newtonsoft.Json;

namespace App.Game.GameManagers.External.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GenerationConfigDto
    {
        [JsonProperty("generations")] 
        private DungeonGenerationConfigDto[] m_Generations;

        public DungeonGenerationConfigDto[] Generations => m_Generations;
    }
}