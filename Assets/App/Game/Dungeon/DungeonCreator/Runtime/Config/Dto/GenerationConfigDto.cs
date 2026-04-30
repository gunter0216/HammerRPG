using System;
using App.Generation.DungeonGenerator.External.Dto;
using Newtonsoft.Json;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Config.Dto
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