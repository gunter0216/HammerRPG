using System;
using System.Collections;
using System.Collections.Generic;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.Runtime.Attributes;
using App.Common.DataContainer.Runtime.Data;
using App.Common.ModuleItem.Runtime.Data;
using Newtonsoft.Json;

namespace App.Common.ModuleItem.External
{
    [Data]
    [Singleton]
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ModuleItemContainerData : IContainerData
    {
        public static string ContainerKey => ModuleItemData.ContainerKey;
        
        [JsonProperty("data")] 
        private List<ModuleItemData> m_Data;

        IList IContainerData.Data => m_Data;
        
        public List<ModuleItemData> Data
        {
            get => m_Data;
            set => m_Data = value;
        }

        public ModuleItemContainerData()
        {
            m_Data = new List<ModuleItemData>();
        }
        
        public string GetContainerKey()
        {
            return ContainerKey;
        }

        public string Name()
        {
            return nameof(ModuleItemContainerData);
        }
    }
}