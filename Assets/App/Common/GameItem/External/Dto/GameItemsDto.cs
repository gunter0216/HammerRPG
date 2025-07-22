using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Common.GameItem.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GameItemsDto
    {
        [JsonProperty("items")] 
        private GameItemDto[] m_Items;
        
        public IReadOnlyList<GameItemDto> Items => m_Items;
    }
}