using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Game.GameTiles.External.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TilesConfigDto
    {
        [JsonProperty("tiles")] 
        private TileConfigDto[] m_Tiles;

        public IReadOnlyList<TileConfigDto> Tiles => m_Tiles;
    }
}