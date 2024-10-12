namespace App.Common.Data.Runtime.Serializer
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T data);
    }
}