using System;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DungeonConfigDto
    {
        [JsonProperty("generation")] 
        private DungeonGenerationConfigDto m_Generation;

        public DungeonGenerationConfigDto Generation => m_Generation;
    }
}