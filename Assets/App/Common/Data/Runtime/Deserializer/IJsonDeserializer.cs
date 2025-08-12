using System;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Data.Runtime.Deserializer
{
    public interface IJsonDeserializer
    {
        Optional<object> Deserialize(string json, Type type);
        Optional<T> Deserialize<T>(string json, Type type);
        Optional<T> Deserialize<T>(string json);
    }
}