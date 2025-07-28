using System;
using Newtonsoft.Json;

namespace App.Game.GameTiles.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TileData
    {
        [JsonProperty("id")]
        private string m_ID;
        
        [JsonProperty("positionX")]
        private int m_PositionX;
        
        [JsonProperty("positionY")]
        private int m_PositionY;

        public string ID
        {
            get => m_ID;
            set => m_ID = value;
        }

        public int PositionX
        {
            get => m_PositionX;
            set => m_PositionX = value;
        }

        public int PositionY
        {
            get => m_PositionY;
            set => m_PositionY = value;
        }
    }
}