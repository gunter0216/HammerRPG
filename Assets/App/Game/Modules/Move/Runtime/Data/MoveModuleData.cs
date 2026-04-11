using System;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Move.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class MoveModuleData : IModuleData
    {
        [JsonProperty("Move")] 
        private string _move;

        public MoveModuleData()
        {
            
        }

        public string Move
        {
            get => _move;
            set => _move = value;
        }

        public string GetModuleKey()
        {
            return MoveContainerData.ContainerKey;
        }
    }
}