using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Stats.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class StatsModuleData : IModuleData
    {
        [JsonProperty("strength")] 
        private string _strength;
        
        [JsonProperty("agility")] 
        private string _agility;
        
        [JsonProperty("intelligence")] 
        private string _intelligence;

        public StatsModuleData()
        {
            
        }

        public string Strength
        {
            get => _strength;
            set => _strength = value;
        }

        public string Agility
        {
            get => _agility;
            set => _agility = value;
        }

        public string Intelligence
        {
            get => _intelligence;
            set => _intelligence = value;
        }

        public string GetModuleKey()
        {
            return StatsContainerData.ContainerKey;
        }
    }
}