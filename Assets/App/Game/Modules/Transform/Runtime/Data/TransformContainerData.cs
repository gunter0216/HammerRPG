using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Transform.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TransformContainerData : IContainerData
    {
        public static string ContainerKey => "TransformContainerData";
        
        [JsonProperty("data")] 
        private List<TransformModuleData> _data;

        IList IContainerData.Data => _data;

        public TransformContainerData()
        {
            _data = new List<TransformModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(TransformContainerData);
        }
    }
}