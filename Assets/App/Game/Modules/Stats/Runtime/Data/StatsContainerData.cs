using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Stats.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class StatsContainerData : IContainerData
    {
        public static string ContainerKey => "StatsContainerData";
        
        [JsonProperty("data")] 
        private List<StatsModuleData> _data;

        IList IContainerData.Data => _data;

        public StatsContainerData()
        {
            _data = new List<StatsModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(StatsContainerData);
        }
    }
}