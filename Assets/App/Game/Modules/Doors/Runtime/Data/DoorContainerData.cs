using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Door.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DoorContainerData : IContainerData
    {
        public static string ContainerKey => "DoorContainerData";
        
        [JsonProperty("data")] 
        private List<DoorModuleData> m_Data;

        IList IContainerData.Data => m_Data;

        public DoorContainerData()
        {
            m_Data = new List<DoorModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(DoorContainerData);
        }
    }
}