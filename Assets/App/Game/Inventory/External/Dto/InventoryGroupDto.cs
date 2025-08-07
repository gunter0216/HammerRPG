using System;
using Newtonsoft.Json;

namespace App.Game.Inventory.External.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryGroupDto
    {
        [JsonProperty("id")]
        private string m_Id;
        [JsonProperty("icon")]
        private string m_Icon;
        [JsonProperty("game_type")]
        private string m_GameType;

        public string Id => m_Id;

        public string Icon => m_Icon;

        public string GameType => m_GameType;
    }
}
