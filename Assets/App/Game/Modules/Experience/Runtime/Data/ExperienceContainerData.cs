using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Experience.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ExperienceContainerData : IContainerData
    {
        public static string ContainerKey => "ExperienceContainerData";
        
        [JsonProperty("data")] 
        private List<ExperienceModuleData> _data;

        IList IContainerData.Data => _data;

        public ExperienceContainerData()
        {
            _data = new List<ExperienceModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(ExperienceContainerData);
        }
    }
}