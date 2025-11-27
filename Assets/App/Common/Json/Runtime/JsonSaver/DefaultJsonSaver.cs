using System;
using System.IO;
using App.Common.Json.Runtime.Serializer;
using UnityEngine;

namespace App.Common.Json.Runtime.JsonSaver
{
    public class DefaultJsonSaver : IJsonSaver
    {
        private readonly IJsonSerializer m_JsonSerializer;

        public DefaultJsonSaver(IJsonSerializer jsonSerializer)
        {
            m_JsonSerializer = jsonSerializer;
        }

        public void Save<T>(T data, string path)
        {
            try
            {
                var json = Serialize(data);
                File.WriteAllText(path, json);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public string Serialize<T>(T data)
        {
            return m_JsonSerializer.Serialize(data);
        }
    }
}