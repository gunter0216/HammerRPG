using System;
using Newtonsoft.Json;

namespace App.Game.GameTiles.External.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TileConfigDto
    {
        [JsonProperty("id")]
        private string m_ID;
        
        [JsonProperty("generation_id")]
        private string m_GenerationID;
        
        [JsonProperty("sprite")]
        private string m_SpriteKey;

        public string ID => m_ID;

        public string GenerationID => m_GenerationID;

        public string SpriteKey => m_SpriteKey;
    }
}