using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.ContainerModule.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ContainerContainerData : IContainerData
    {
        public static string ContainerKey => "ContainerContainerData";
        
        [JsonProperty("data")] 
        private List<ContainerModuleData> m_Data;

        IList IContainerData.Data => m_Data;

        public ContainerContainerData()
        {
            m_Data = new List<ContainerModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(ContainerContainerData);
        }
    }
}