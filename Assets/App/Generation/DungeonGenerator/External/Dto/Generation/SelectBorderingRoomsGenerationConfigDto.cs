using System;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto.Generation
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SelectBorderingRoomsGenerationConfigDto
    {
        [SerializeField, JsonProperty("Min Corridor Size")]
        private int m_MinCorridorSize = 3;

        public int MinCorridorSize => m_MinCorridorSize;
    }
}