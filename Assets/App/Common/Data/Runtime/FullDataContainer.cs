using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Common.Data.Runtime
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class FullDataContainer
    {
        [SerializeField, JsonProperty("datas")] private List<DataWrapper> m_Datas;

        public List<DataWrapper> Datas
        {
            get => m_Datas;
            set => m_Datas = value;
        }
        
        public FullDataContainer()
        {
            
        }
        
        public FullDataContainer(List<DataWrapper> datas)
        {
            m_Datas = datas;
        }
    }
}