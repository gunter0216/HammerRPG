using System;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using Newtonsoft.Json;

namespace App.Common.Data.Runtime.Deserializer
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
                HLogger.LogError(e.Message);
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
                HLogger.LogError(e.Message);
            }
            
            return Optional<T>.Empty;
        }
        
        public Optional<T> Deserialize<T>(string json)
        {
            return Deserialize<T>(json, typeof(T));
        }
    }
}