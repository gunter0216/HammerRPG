using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Stats.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class StatsModuleData : IModuleData
    {
        [JsonProperty("stats")] 
        private string _stats;

        public StatsModuleData()
        {
            
        }

        public string Stats
        {
            get => _stats;
            set => _stats = value;
        }

        public string GetModuleKey()
        {
            return StatsContainerData.ContainerKey;
        }
    }
}