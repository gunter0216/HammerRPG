using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Health.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class HealthModuleData : IModuleData
    {
        [JsonProperty("health")] 
        private int _health;

        public HealthModuleData()
        {
            
        }

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public string GetModuleKey()
        {
            return HealthContainerData.ContainerKey;
        }
    }
}