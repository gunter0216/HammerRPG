using System;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Common.Data.Runtime
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class DataWrapper
    {
        [SerializeField, JsonProperty("type")] private string m_Type;
        [SerializeField, JsonProperty("object")] private string m_Object;

        public string Type
        {
            get => m_Type;
            set => m_Type = value;
        }
        
        public string Object
        {
            get => m_Object;
            set => m_Object = value;
        }
        
        public DataWrapper()
        {
            
        }
        
        public DataWrapper(string type, string obj)
        {
            m_Type = type;
            m_Object = obj;
        }
    }
}