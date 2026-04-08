using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Name.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class NameModuleData : IModuleData
    {
        [JsonProperty("name")] 
        private string _name;

        public NameModuleData()
        {
            
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string GetModuleKey()
        {
            return NameContainerData.ContainerKey;
        }
    }
}