using System;
using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using Newtonsoft.Json;

namespace App.Common.GameItem.Runtime.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GameItemData : IGameItemData
    {
        [JsonProperty("id")] private string m_Id;
        [JsonProperty("modules")] private List<DataReference> m_ModuleRefs;
        
        public string Id => m_Id;
        public List<DataReference> ModuleRefs => m_ModuleRefs;

        public GameItemData()
        {
            m_ModuleRefs = new List<DataReference>();
        }
        
        public GameItemData(string id, List<DataReference> moduleRefs)
        {
            m_Id = id;
            m_ModuleRefs = moduleRefs;
        }
    }
}