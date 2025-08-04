using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.Runtime.Attributes;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.GameTiles.External.Config.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class PositionContainerData : IContainerData
    {
        public static string ContainerKey => "PositionContainerData";
        
        [JsonProperty("data")] 
        private List<TilePositionModuleData> m_Data;

        IList IContainerData.Data => m_Data;
        
        public List<TilePositionModuleData> Data
        {
            get => m_Data;
            set => m_Data = value;
        }

        public PositionContainerData()
        {
            m_Data = new List<TilePositionModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(PositionContainerData);
        }
    }
}