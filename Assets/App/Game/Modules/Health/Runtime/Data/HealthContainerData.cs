using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Health.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class HealthContainerData : IContainerData
    {
        public static string ContainerKey => "HealthContainerData";
        
        [JsonProperty("data")] 
        private List<HealthModuleData> _data;

        IList IContainerData.Data => _data;

        public HealthContainerData()
        {
            _data = new List<HealthModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(HealthContainerData);
        }
    }
}