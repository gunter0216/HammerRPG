using System;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Json.Runtime.Deserializer
{
    public interface IJsonDeserializer
    {
        Optional<object> Deserialize(string json, Type type);
        Optional<T> Deserialize<T>(string json, Type type);
        Optional<T> Deserialize<T>(string json);
    }
}