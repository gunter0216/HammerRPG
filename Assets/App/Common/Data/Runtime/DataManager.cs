using System;
using System.Collections.Generic;
using System.IO;
using App.Common.Data.Runtime.JsonLoader;
using App.Common.Data.Runtime.JsonSaver;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Data.Runtime
{
    public class DataManager : IDataManager
    {
        private const string m_FileName = "Save.json";

        private readonly IDataSavePathCreator m_DataSavePathCreator;
        private readonly IJsonLoader m_Loader;
        private readonly IJsonSaver m_Saver;
        
        private string m_SaveDirectory;
        private string m_FilePath;

        private List<IData> m_Datas;
        private Dictionary<string, IData> m_NameToData;
        private Dictionary<string, Type> m_DataToType;

        private bool m_IsInitialized = false;

        public DataManager(IJsonLoader loader, IJsonSaver saver, IDataSavePathCreator dataSavePathCreator)
        {
            m_Loader = loader;
            m_Saver = saver;
            m_DataSavePathCreator = dataSavePathCreator;
        }

        public void Init()
        {
            if (m_IsInitialized)
            {
                return;
            }

            m_IsInitialized = true;
            
            m_SaveDirectory = m_DataSavePathCreator.Create();
            m_FilePath = Path.Combine(m_SaveDirectory, m_FileName);
            
            m_NameToData = new Dictionary<string, IData>(m_Datas.Count);
            m_DataToType = new Dictionary<string, Type>(m_Datas.Count);
            
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

        public void SetDatas(List<IData> datas)
        {
            m_Datas = datas;
        }

        public void SaveProgress()
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
        
        public Optional<T> GetData<T>(string name) where T : IData
        {
            if (m_NameToData.TryGetValue(name, out var data))
            {
                if (data is not T typedData)
                {
                    HLogger.LogError($"Data {name} is not of type {typeof(T)}");
                    return Optional<T>.Fail();
                }
                
                return new Optional<T>(typedData);
            }
            
            HLogger.LogError($"Data not found {name}");
            return Optional<T>.Fail();
        }

        private void CreateNewDatas()
        {
            foreach (var data in m_Datas)
            {
                if (!m_NameToData.ContainsKey(data.Name()))
                {
                    // data.WhenCreateNewData();
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
                // data.BeforeSerialize();
                var obj = m_Saver.Serialize(data);
                var dataWrapper = new DataWrapper(data.Name(), obj);
                dataWrappers.Add(dataWrapper);
            }
            
            var fullData = new FullDataContainer(dataWrappers);
            m_Saver.Save(fullData, path);
        }
    }
}