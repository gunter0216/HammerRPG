using System;
using System.IO;
using App.Common.Json.Runtime.Deserializer;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Common.Json.Runtime.JsonLoader
{
    public class DefaultJsonLoader : IJsonLoader
    {
        private readonly IJsonDeserializer m_Deserializer;

        public DefaultJsonLoader(IJsonDeserializer deserializer)
        {
            m_Deserializer = deserializer;
        }

        public Optional<T> Load<T>(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"File is not exists {path}");
                return Optional<T>.Empty;
            }
            
            try
            {
                var json = File.ReadAllText(path);
                var data = Deserialize<T>(json);
                if (!data.HasValue)
                {
                    Debug.LogError($"Cant deserialize {json}");
                    return Optional<T>.Empty;
                }
                
                return new Optional<T>(data.Value);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            
            return Optional<T>.Empty;
        }


        public Optional<object> Deserialize(string json, Type type)
        {
            return m_Deserializer.Deserialize(json, type);
        }

        public Optional<T> Deserialize<T>(string json, Type type)
        {
            return m_Deserializer.Deserialize<T>(json, type);
        }

        public Optional<T> Deserialize<T>(string json)
        {
            return m_Deserializer.Deserialize<T>(json);
        }
    }
}