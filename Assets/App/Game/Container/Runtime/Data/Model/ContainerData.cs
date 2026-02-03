using System;
using System.Collections.Generic;
using App.Common.Data.Runtime;
using Newtonsoft.Json;

namespace App.Game.Container.Runtime.Data.Model
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ContainerData : IData
    {
        [JsonProperty("items")] private List<ContainerItemData> m_Items;
        [JsonProperty("guid")] private int m_Guid;
        [JsonProperty("length")] private int m_Length;

        public List<ContainerItemData> Items
        {
            get => m_Items;
            set => m_Items = value;
        }

        public int Guid
        {
            get => m_Guid;
            set => m_Guid = value;
        }

        public int Length
        {
            get => m_Length;
            set => m_Length = value;
        }

        public ContainerData()
        {
            
        }

        public string Name()
        {
            return nameof(ContainersData);
        }
    }
}