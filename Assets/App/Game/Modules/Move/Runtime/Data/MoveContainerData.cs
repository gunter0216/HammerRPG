using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Move.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class MoveContainerData : IContainerData
    {
        public static string ContainerKey => "MoveContainerData";
        
        [JsonProperty("data")] 
        private List<MoveModuleData> _data;

        IList IContainerData.Data => _data;

        public MoveContainerData()
        {
            _data = new List<MoveModuleData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(MoveContainerData);
        }
    }
}