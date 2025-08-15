using System;
using Newtonsoft.Json;

namespace App.Game.ModuleItemType.Runtime.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GameItemTypeModuleDto
    {
        [JsonProperty("type")]
        private string m_Type;

        public string Type => m_Type;
    }
}