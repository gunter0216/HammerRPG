using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Race.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class RaceContainerData : IContainerData
    {
        public static string ContainerKey => "RaceContainerData";
        
        [JsonProperty("data")] 
        private List<RaceModuleData> _data;

        IList IContainerData.Data => _data;

        public RaceContainerData()
        {
            _data = new List<RaceModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(RaceContainerData);
        }
    }
}