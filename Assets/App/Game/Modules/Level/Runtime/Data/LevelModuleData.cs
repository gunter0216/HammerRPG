using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Level.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class LevelModuleData : IModuleData
    {
        [JsonProperty("level")] 
        private int _level;

        public LevelModuleData()
        {
            
        }

        public int Level
        {
            get => _level;
            set => _level = value;
        }

        public string GetModuleKey()
        {
            return LevelContainerData.ContainerKey;
        }
    }
}