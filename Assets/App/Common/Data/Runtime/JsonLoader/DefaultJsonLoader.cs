using System;
using System.IO;
using App.Common.Data.Runtime.Deserializer;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using UnityEngine;

namespace App.Common.Data.Runtime.JsonLoader
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
                HLogger.LogError($"File is not exists {path}");
                return Optional<T>.Empty;
            }
            
            try
            {
                var json = File.ReadAllText(path);
                var data = Deserialize<T>(json);
                if (!data.HasValue)
                {
                    HLogger.LogError($"Cant deserialize {json}");
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