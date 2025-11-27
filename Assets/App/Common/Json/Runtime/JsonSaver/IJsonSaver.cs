using App.Common.Json.Runtime.Serializer;

namespace App.Common.Json.Runtime.JsonSaver
{
    public interface IJsonSaver : IJsonSerializer
    {
        void Save<T>(T data, string path);
    }
}