using App.Common.Data.Runtime.Serializer;

namespace App.Common.Data.Runtime.JsonSaver
{
    public interface IJsonSaver : IJsonSerializer
    {
        void Save<T>(T data, string path);
    }
}