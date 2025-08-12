using System;
using Newtonsoft.Json;

namespace App.Common.ModuleItem.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ModuleItemModuleDto
    {
        [JsonProperty("key")]
        private string m_Key;
        
        [JsonProperty("content")]
        private string m_Content;
        
        public string Key => m_Key;
        public string Content => m_Content;
    }
}