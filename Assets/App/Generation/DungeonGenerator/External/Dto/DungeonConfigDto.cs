using System;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DungeonConfigDto
    {
        [SerializeField, JsonProperty("generation")] 
        private DungeonGenerationConfigDto m_Generation;

        public DungeonGenerationConfigDto Generation => m_Generation;
    }
}