using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.TilePosition.Runtime
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TilePositionContainerData : IContainerData
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

        public TilePositionContainerData()
        {
            m_Data = new List<TilePositionModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(TilePositionContainerData);
        }
    }
}