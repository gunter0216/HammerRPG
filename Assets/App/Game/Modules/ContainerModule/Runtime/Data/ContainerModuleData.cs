using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.ContainerModule.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ContainerModuleData : IModuleData
    {
        [JsonProperty("guid")] 
        private int _containerGuid;

        public ContainerModuleData()
        {
        }
        
        public ContainerModuleData(int containerGuid)
        {
            _containerGuid = containerGuid;
        }

        public int ContainerGuid
        {
            get => _containerGuid;
            set => _containerGuid = value;
        }

        public string GetModuleKey()
        {
            return ContainerContainerData.ContainerKey;
        }
    }
}