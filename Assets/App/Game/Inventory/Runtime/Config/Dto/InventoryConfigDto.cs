using System;
using Newtonsoft.Json;

namespace App.Game.Inventory.Runtime.Config.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryConfigDto
    {
        [JsonProperty("cols")]
        private int _cols;
        [JsonProperty("rows")]
        private int _rows;

        public int Cols => _cols;
        public int Rows => _rows;
    }
}
