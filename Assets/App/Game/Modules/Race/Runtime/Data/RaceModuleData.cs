using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Race.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class RaceModuleData : IModuleData
    {
        [JsonProperty("race")] 
        private string _race;

        public RaceModuleData()
        {
            
        }

        public string Race
        {
            get => _race;
            set => _race = value;
        }

        public string GetModuleKey()
        {
            return RaceContainerData.ContainerKey;
        }
    }
}