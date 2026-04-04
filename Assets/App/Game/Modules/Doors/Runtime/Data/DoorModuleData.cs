using System;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Door.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DoorModuleData : IModuleData
    {
        [JsonProperty("state")] 
        private int _state;
        
        [JsonProperty("key")] 
        private DataReference _key;

        public DoorModuleData()
        {
            
        }
        
        public DoorState State
        {
            get => (DoorState)_state;
            set => _state = (int)value;
        }

        public DataReference Key
        {
            get => _key;
            set => _key = value;
        }

        public string GetModuleKey()
        {
            return DoorContainerData.ContainerKey;
        }
    }
}