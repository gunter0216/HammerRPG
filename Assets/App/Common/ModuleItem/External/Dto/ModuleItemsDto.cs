using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Common.ModuleItem.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GameItemsDto
    {
        [JsonProperty("items")] 
        private ModuleItemDto[] m_Items;
        
        public IReadOnlyList<ModuleItemDto> Items => m_Items;
    }
}