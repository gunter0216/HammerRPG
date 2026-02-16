using System;
using App.Common.DataContainer.Runtime;
using Newtonsoft.Json;

namespace App.Generation.DungeonCreator.Runtime.Tiles
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TileData
    {
        [JsonProperty("reference")] 
        private DataReference m_Reference;

        public DataReference Reference
        {
            get => m_Reference;
            set => m_Reference = value;
        }
    }
}