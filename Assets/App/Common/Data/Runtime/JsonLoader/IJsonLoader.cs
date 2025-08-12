using App.Common.Data.Runtime.Deserializer;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Data.Runtime.JsonLoader
{
    public interface IJsonLoader : IJsonDeserializer
    {
        Optional<T> Load<T>(string path);
    }
}