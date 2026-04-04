using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Generation.DungeonCreator.Runtime.Chest
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ChestModuleData : IModuleData
    {
        [JsonProperty("state")] 
        private int _state;

        public ChestModuleData()
        {
            
        }
        
        public ChestState State
        {
            get => (ChestState)_state;
            set => _state = (int)value;
        }

        public string GetModuleKey()
        {
            return ChestContainerData.ContainerKey;
        }
    }
}