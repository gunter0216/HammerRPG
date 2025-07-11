using System;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Generation.DungeonGenerator.External.Dto.Common
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SizeIntDto
    {
        [SerializeField, JsonProperty("Width")]
        private int m_Width;
        
        [SerializeField, JsonProperty("Height")]
        private int m_Height;

        public int Width => m_Width;

        public int Height => m_Height;
    }
}