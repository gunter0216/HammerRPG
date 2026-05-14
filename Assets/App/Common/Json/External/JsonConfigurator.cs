using System.Collections.Generic;
using App.Common.Json.Runtime.Deserializer;
using App.Common.Json.Runtime.JsonLoader;
using App.Common.Json.Runtime.JsonSaver;
using App.Common.Json.Runtime.Serializer;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using Newtonsoft.Json;

namespace App.Common.Json.External
{
    [Configurator(DIContext.GlobalContext)]    
    public class JsonConfigurator : Core.Startups.External.Configurator
    {
        private JsonSerializerSettings _settings;

        public override void Configuration()
        {
            Container.Bind<IJsonLoader>().FromInstance(BeanJsonLoader());
            Container.Bind<IJsonSaver>().FromInstance(BeanJsonSaver());
            Container.Bind<IJsonDeserializer>().FromInstance(GetJsonDeserializer());
            Container.Bind<IJsonSerializer>().FromInstance(BeanJsonSerializer());
        }

        public JsonSerializerSettings GetJsonSerializerSettings()
        {
            _settings ??= new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "d.M.yyyy HH:mm:ss",
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>()
                {
                    new Vector2Converter(),
                    new Vector2IntConverter()
                }
            };
            
            return _settings;
        }

        public IJsonLoader BeanJsonLoader()
        {
            return new DefaultJsonLoader(GetJsonDeserializer());
        }

        private IJsonSaver BeanJsonSaver()
        {
            return new DefaultJsonSaver(BeanJsonSerializer());
        }

        public IJsonDeserializer GetJsonDeserializer()
        {
            return new NewtonsoftJsonDeserializer(GetJsonSerializerSettings());
        }

        public IJsonSerializer BeanJsonSerializer()
        {
            return new NewtonsoftJsonSerializer(GetJsonSerializerSettings());
        }
    }
}