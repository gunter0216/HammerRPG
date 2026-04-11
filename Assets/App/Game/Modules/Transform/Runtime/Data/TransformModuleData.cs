using System;
using App.Common.Algorithms.Runtime;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Game.Modules.Transform.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TransformModuleData : IModuleData
    {
        [JsonProperty("position")] 
        private Vector2 _position;

        public TransformModuleData()
        {
            
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public string GetModuleKey()
        {
            return TransformContainerData.ContainerKey;
        }
    }
}