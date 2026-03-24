using System;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SeparateRoomsGenerationConfigDto
    {
        [JsonProperty("speed")] [SerializeField]
        private int m_Speed = 1;

        public int Speed => m_Speed;
    }
}