using System;
using Newtonsoft.Json;

namespace App.Common.DataContainer.Runtime
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DataReference : IDataReference
    {
        [JsonProperty("key")] 
        private string m_Key;
        
        [JsonProperty("index")] 
        private int m_Index;

        public string Key => m_Key;
        public int Index => m_Index;

        public DataReference()
        {
            
        }
        
        public DataReference(string key, int index)
        {
            m_Key = key;
            m_Index = index;
        }
    }
}