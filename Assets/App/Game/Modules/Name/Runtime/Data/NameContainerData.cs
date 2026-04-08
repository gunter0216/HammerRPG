using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Name.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class NameContainerData : IContainerData
    {
        public static string ContainerKey => "NameContainerData";
        
        [JsonProperty("data")] 
        private List<NameModuleData> _data;

        IList IContainerData.Data => _data;

        public NameContainerData()
        {
            _data = new List<NameModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(NameContainerData);
        }
    }
}