using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Chests.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ChestContainerData : IContainerData
    {
        public static string ContainerKey => "ChestContainerData";
        
        [JsonProperty("data")] 
        private List<ChestModuleData> m_Data;

        IList IContainerData.Data => m_Data;

        public ChestContainerData()
        {
            m_Data = new List<ChestModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(ChestContainerData);
        }
    }
}