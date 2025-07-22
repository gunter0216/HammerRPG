using System;
using Newtonsoft.Json;

namespace App.Common.GameItem.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GameItemDto
    {
        [JsonProperty("id")] private string m_Id;
        [JsonProperty("tags")] private long m_Tags;
        [JsonProperty("modules")] private GameItemModulesDto m_Modules;

        public string Id => m_Id;
        public long Tags => m_Tags;
        public GameItemModulesDto Modules => m_Modules;
    }
}