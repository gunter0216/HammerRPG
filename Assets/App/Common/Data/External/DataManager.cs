using System;
using System.Collections.Generic;
using System.IO;
using App.Common.AssemblyManager.Runtime;
using App.Common.Data.Runtime;
using App.Common.Data.Runtime.Deserializer;
using App.Common.Data.Runtime.JsonLoader;
using App.Common.Data.Runtime.JsonSaver;
using App.Common.Data.Runtime.Serializer;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using App.Game;
using App.Game.States.Start;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Common.Data.External
{
    [Singleton]
    [Stage(typeof(StartInitPhase), -100_000)]
    public class DataManager : IDataManager, IInitSystem
    {
        private const string m_FileName = "Save.json";
        
        private string m_SaveDirectory;
        private string m_FilePath;

        private IJsonLoader m_Loader;
        private IJsonSaver m_Saver;

        private List<IData> m_Datas;
        private Dictionary<string, IData> m_NameToData;
        private Dictionary<string, Type> m_DataToType;

        private bool m_IsInitialized = false;
        
        // todo добавить в конфигурато saver and loader
        public void Init()
        {
            if (m_IsInitialized)
            {
                return;
            }

            m_IsInitialized = true;
            
            m_SaveDirectory = Path.Combine(Application.persistentDataPath, "Data");
            m_FilePath = Path.Combine(m_SaveDirectory, m_FileName);
            
            m_NameToData = new Dictionary<string, IData>(m_Datas.Count);
            m_DataToType = new Dictionary<string, Type>(m_Datas.Count);
            
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

            foreach (var data in m_Datas)
            {
                m_DataToType.Add(data.Name(), data.GetType());
            }
            
            Load(m_FilePath);
            CreateNewDatas();
        }

        public void SetDatas(List<AttributeNode> datas)
        {
            m_Datas = new List<IData>(datas.Count);
            for (int i = 0; i < datas.Count; ++i)
            {
                var holder = datas[i].Holder;
                var instance = Activator.CreateInstance(holder) as IData;
                if (instance == null)
                {
                    HLogger.LogError($"data {datas[i].Holder.Name} contains attribute but no interface");
                    continue;
                }
                
                m_Datas.Add(instance);
            }
        }

        public void SaveByExit()
        {
            HLogger.Log("SaveByExit");
            Save(m_FilePath);
        }

        public Optional<IData> GetData(string name)
        {
            if (m_NameToData.TryGetValue(name, out var data))
            {
                return new Optional<IData>(data);
            }
            
            HLogger.LogError($"Data not found {name}");
            return Optional<IData>.Empty;
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
                if (!m_DataToType.TryGetValue(dataWrapper.Type, out var dataType))
                {
                    HLogger.LogError($"Not found type");
                    continue;
                }
                
                var data = m_Loader.Deserialize<IData>(dataWrapper.Object, dataType);
                if (!data.HasValue)
                {
                    HLogger.LogError($"Cant deserialize {dataWrapper.Object}");
                    return;
                }
                
                m_NameToData.Add(data.Value.Name(), data.Value);
            }
        }

        private void Save(string path)
        {
            var dataWrappers = new List<DataWrapper>();
            foreach (var data in m_NameToData.Values)
            {
                data.BeforeSerialize();
                var obj = m_Saver.Serialize(data);
                var dataWrapper = new DataWrapper(data.Name(), obj);
                dataWrappers.Add(dataWrapper);
            }
            
            var fullData = new FullDataContainer(dataWrappers);
            m_Saver.Save(fullData, path);
        }
    }
}