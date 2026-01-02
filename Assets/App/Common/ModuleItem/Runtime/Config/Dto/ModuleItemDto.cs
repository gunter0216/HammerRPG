using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Common.ModuleItem.Runtime.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ModuleItemDto
    {
        [JsonProperty("id")] private string m_Id;
        [JsonProperty("tags")] private long m_Tags;
        [JsonProperty("modules")] private Dictionary<string, string>[] m_Modules;

        public string Id => m_Id;
        public long Tags => m_Tags;
        public Dictionary<string, string>[] Modules => m_Modules;
    }
}