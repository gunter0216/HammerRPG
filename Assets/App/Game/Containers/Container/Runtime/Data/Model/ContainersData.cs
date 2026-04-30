using System;
using System.Collections.Generic;
using App.Common.Data.Runtime;
using Newtonsoft.Json;

namespace App.Game.Containers.Container.Runtime.Data.Model
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ContainersData : IData
    {
        [JsonProperty("containers")] private List<ContainerData> m_Containers;

        public List<ContainerData> Containers
        {
            get => m_Containers;
            set => m_Containers = value;
        }

        public string Name()
        {
            return nameof(ContainersData);
        }
    }
}