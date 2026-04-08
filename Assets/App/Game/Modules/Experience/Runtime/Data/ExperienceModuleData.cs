using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Experience.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ExperienceModuleData : IModuleData
    {
        [JsonProperty("experience")] 
        private int _experience;

        public ExperienceModuleData()
        {
            
        }

        public int Experience
        {
            get => _experience;
            set => _experience = value;
        }

        public string GetModuleKey()
        {
            return ExperienceContainerData.ContainerKey;
        }
    }
}