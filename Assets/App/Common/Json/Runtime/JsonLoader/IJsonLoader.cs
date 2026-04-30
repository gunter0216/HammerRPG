using App.Common.Json.Runtime.Deserializer;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Json.Runtime.JsonLoader
{
    public interface IJsonLoader : IJsonDeserializer
    {
        Optional<T> Load<T>(string path);
    }
}