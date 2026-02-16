using System;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.External.Dto.Common
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SizeIntDto
    {
        [JsonProperty("width")]
        private int m_Width;
        
        [JsonProperty("height")]
        private int m_Height;

        public int Width => m_Width;

        public int Height => m_Height;
    }
}