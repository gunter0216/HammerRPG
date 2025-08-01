using System;
using Newtonsoft.Json;

namespace App.Common.ModuleItem.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ModuleItemDto
    {
        [JsonProperty("id")] private string m_Id;
        [JsonProperty("tags")] private long m_Tags;
        [JsonProperty("modules")] private ModuleItemModulesDto m_Modules;

        public string Id => m_Id;
        public long Tags => m_Tags;
        public ModuleItemModulesDto Modules => m_Modules;
    }
}