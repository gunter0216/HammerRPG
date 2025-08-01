using System;
using Newtonsoft.Json;

namespace App.Game.GameTiles.External.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TileSpriteModuleDto
    {
        [JsonProperty("key")]
        private string m_Key;

        public string Key => m_Key;
    }
}