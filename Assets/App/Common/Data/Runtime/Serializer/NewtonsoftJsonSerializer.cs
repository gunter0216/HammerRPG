using Newtonsoft.Json;

namespace App.Common.Data.Runtime.Serializer
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings m_Settings;

        public NewtonsoftJsonSerializer(JsonSerializerSettings settings)
        {
            m_Settings = settings;
        }

        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data, m_Settings);
        }
    }
}