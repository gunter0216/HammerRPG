using System;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using Newtonsoft.Json;

namespace App.Common.Data.Runtime.Deserializer
{
    public class NewtonsoftJsonDeserializer : IJsonDeserializer
    {
        private readonly JsonSerializerSettings m_Settings;

        public NewtonsoftJsonDeserializer(JsonSerializerSettings settings)
        {
            m_Settings = settings;
        }

        public Optional<T> Deserialize<T>(string json)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<T>(json, m_Settings);
                return new Optional<T>(item);
            }
            catch (Exception e)
            {
                HLogger.LogError(e.Message);
            }
            
            return Optional<T>.Empty;
        }
    }
}