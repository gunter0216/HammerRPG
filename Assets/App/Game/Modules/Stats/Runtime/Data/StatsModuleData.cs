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
        private int _strength;
        
        [JsonProperty("agility")] 
        private int _agility;
        
        [JsonProperty("intelligence")] 
        private int _intelligence;

        public StatsModuleData()
        {
            
        }

        public int Strength
        {
            get => _strength;
            set => _strength = value;
        }

        public int Agility
        {
            get => _agility;
            set => _agility = value;
        }

        public int Intelligence
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