using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.GameTiles.External.Config.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TilePositionModuleData : IModuleData
    {
        [JsonProperty("position_x")]
        private int m_PositionX;

        [JsonProperty("position_y")]
        private int m_PositionY;

        public int PositionX => m_PositionX;

        public int PositionY => m_PositionY;

        public TilePositionModuleData()
        {
            
        }
        
        public TilePositionModuleData(int positionX, int positionY)
        {
            m_PositionX = positionX;
            m_PositionY = positionY;
        }

        public string GetModuleKey()
        {
            return PositionContainerData.ContainerKey;
        }
    }
}