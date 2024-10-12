using System.Collections.Generic;
using System.IO;
using App.Common.Data.Runtime;
using App.Common.Data.Runtime.Deserializer;
using App.Common.Data.Runtime.JsonLoader;
using App.Common.Data.Runtime.JsonSaver;
using App.Common.Data.Runtime.Serializer;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Common.Data.External
{
    [Singleton]
    public class DataManager : IDataManager
    {
        private const string m_FileName = "Save";
        
        private readonly string m_SaveDirectory;
        private readonly string m_FilePath;
        
        [Inject] private List<IData> m_Datas;
        
        private readonly IJsonLoader m_Loader;
        private readonly IJsonSaver m_Saver;

        private Dictionary<string, IData> m_NameToData;
        
        // todo добавить в конфигурато saver and loader
        public DataManager()
        {
            m_SaveDirectory = Path.Combine(Application.persistentDataPath, "Data");
            m_FilePath = Path.Combine(m_SaveDirectory, m_FileName);
            
            m_NameToData = new Dictionary<string, IData>(m_Datas.Count);
            
            var jsonSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "d.M.yyyy HH:mm:ss",
                Formatting = Formatting.Indented
            };
            
            IJsonDeserializer deserializer = new NewtonsoftJsonDeserializer(jsonSettings);
            IJsonSerializer serializer = new NewtonsoftJsonSerializer(jsonSettings);
            m_Loader = new DefaultJsonLoader(deserializer);
            m_Saver = new DefaultJsonSaver(serializer);
            
            if (!Directory.Exists(m_SaveDirectory))
            {
                Directory.CreateDirectory(m_SaveDirectory);
            }

            Load(m_FilePath);
            CreateNewDatas();
        }

        public void SaveByExit()
        {
            Save(m_FilePath);
        }

        private void CreateNewDatas()
        {
            foreach (var data in m_Datas)
            {
                if (!m_NameToData.ContainsKey(data.Name()))
                {
                    data.WhenCreateNewData();
                    m_NameToData.Add(data.Name(), data);
                }
            }
        }

        private void Load(string path)
        {
            if (!File.Exists(path))
            {
                HLogger.LogError($"Not found file {path}");
                return;
            }
            
            var fullData = m_Loader.Load<FullDataContainer>(path);
            if (!fullData.HasValue)
            {
                HLogger.LogError($"Cant load data {path}");
                return;
            }

            m_NameToData.Clear();
            foreach (var dataWrapper in fullData.Value.Datas)
            {
                var data = m_Loader.Deserialize<IData>(dataWrapper.Object);
                if (!data.HasValue)
                {
                    HLogger.LogError($"Cat deserialize {dataWrapper.Object}");
                    return;
                }
                
                m_NameToData.Add(data.Value.Name(), data.Value);
            }
        }

        private void Save(string path)
        {
            var dataWrappers = new List<DataWrapper>();
            foreach (var data in m_Datas)
            {
                data.BeforeSerialize();
                var obj = m_Saver.Serialize(data);
                var dataWrapper = new DataWrapper(data.Name(), obj);
                dataWrappers.Add(dataWrapper);
            }
            
            var fullData = new FullDataContainer(dataWrappers);
            m_Saver.Save(fullData, path);
        }

        private Optional<IData> GetData(string name)
        {
            if (m_NameToData.TryGetValue(name, out var data))
            {
                return new Optional<IData>(data);
            }
            
            HLogger.LogError($"Data not found {name}");
            return Optional<IData>.Empty;
        }
    }
}