using System;
using App.Generation.DungeonGenerator.External.Dto.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TileBasedGenerationConfigDto
    {
        [JsonProperty("size")] [SerializeField]
        private SizeIntDto _size;
        
        
        public SizeIntDto Size => _size;
    }
}