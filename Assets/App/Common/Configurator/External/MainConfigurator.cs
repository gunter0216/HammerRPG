﻿using App.Common.Data.Runtime.Deserializer;
using App.Common.Data.Runtime.JsonLoader;
using App.Common.Data.Runtime.JsonSaver;
using App.Common.Data.Runtime.Serializer;
using App.Common.HammerDI.Runtime.Attributes;
using Newtonsoft.Json;

namespace App.Common.Configurator.External
{
    [Configurator]
    public class MainConfigurator
    {
        [Singleton]
        public JsonSerializerSettings BeanJsonSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "d.M.yyyy HH:mm:ss",
                Formatting = Formatting.Indented
            };
        }

        [Singleton]
        public IJsonDeserializer BeanJsonDeserializer()
        {
            return new NewtonsoftJsonDeserializer(BeanJsonSerializerSettings());
        }
        
        [Singleton]
        public IJsonSerializer BeanJsonSerializer()
        {
            return new NewtonsoftJsonSerializer(BeanJsonSerializerSettings());
        }
        
        [Singleton]
        public IJsonLoader BeanJsonLoader()
        {
            return new DefaultJsonLoader(BeanJsonDeserializer());
        }
        
        [Singleton]
        public IJsonSaver BeanJsonSaver()
        {
            return new DefaultJsonSaver(BeanJsonSerializer());
        }
    }
}