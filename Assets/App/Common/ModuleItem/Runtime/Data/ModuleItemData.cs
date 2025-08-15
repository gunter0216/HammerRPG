using System;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using Newtonsoft.Json;

namespace App.Common.ModuleItem.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ModuleItemData : IModuleItemData, IModuleData
    {
        public static string ContainerKey => "ModuleItemData";
        
        [JsonProperty("id")] private string m_Id;
        [JsonProperty("modules")] private List<DataReference> m_ModuleRefs;
        
        public string Id => m_Id;
        public List<DataReference> ModuleRefs => m_ModuleRefs;

        public ModuleItemData()
        {
            m_ModuleRefs = new List<DataReference>();
        }
        
        public ModuleItemData(string id, List<DataReference> moduleRefs)
        {
            m_Id = id;
            m_ModuleRefs = moduleRefs;
        }

        public string GetModuleKey()
        {
            return ContainerKey;
        }

        public override string ToString()
        {
            return $"ModuleItemData(Id: {m_Id}, ModuleRefs: [{string.Join(", ", m_ModuleRefs)}])";
        }
    }
}