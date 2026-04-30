using System;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Common.Json.Runtime.Deserializer
{
    public class NewtonsoftJsonDeserializer : IJsonDeserializer
    {
        private readonly JsonSerializerSettings m_Settings;
        private IJsonDeserializer m_JsonDeserializerImplementation;

        public NewtonsoftJsonDeserializer(JsonSerializerSettings settings)
        {
            m_Settings = settings;
        }

        public Optional<object> Deserialize(string json, Type type)
        {
            try
            {
                var item = JsonConvert.DeserializeObject(json, type, m_Settings);
                return new Optional<object>(item);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            
            return Optional<object>.Empty;
        }

        public Optional<T> Deserialize<T>(string json, Type type)
        {
            try
            {
                var item = (T)JsonConvert.DeserializeObject(json, type, m_Settings);
                return new Optional<T>(item);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            
            return Optional<T>.Empty;
        }
        
        public Optional<T> Deserialize<T>(string json)
        {
            return Deserialize<T>(json, typeof(T));
        }
    }
}