namespace App.Common.Json.Runtime.Serializer
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T data);
    }
}