using System.Collections.Generic;
using App.Common.DataContainer.Runtime.Data;
using App.Common.DataContainer.Runtime.Data.Loader;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.DataContainer.Runtime
{
    // todo если среднее кол-во дат будет большим, добавить список пустых слотов для каждого контейнера, изменив сложность добавления с O(n) до O(1)
    public class ContainersDataManager : IContainersDataManager
    {
        private readonly IContainerDataLoader m_DataLoader;
        private readonly ILogger m_Logger;

        private Dictionary<string, IContainerData> m_DataContainers;

        public ContainersDataManager(IContainerDataLoader dataLoader, ILogger logger)
        {
            m_DataLoader = dataLoader;
            m_Logger = logger;
        }

        public bool Initialize()
        {
            var containers = m_DataLoader.Load();
            if (!containers.HasValue)
            {
                return false;
            }

            m_DataContainers = new Dictionary<string, IContainerData>(containers.Value.Count);
            for (int i = 0; i < containers.Value.Count; ++i)
            {
                AddContainer(containers.Value[i]);
            }
            
            return true;
        }

        public void AddContainer(IContainerData container)
        {
            m_DataContainers.Add(container.GetContainerKey(), container);
        }

        public Optional<DataReference> AddData(string key, object data)
        {
            if (data == null)
            {
                m_Logger.LogError("Data cannot be null.");
                return Optional<DataReference>.Fail();
            }
            
            if (string.IsNullOrEmpty(key))
            {
                m_Logger.LogError("Key cannot be null or empty.");
                return Optional<DataReference>.Fail();
            }
            
            if (!TryGetContainer(key, out var containerData))
            {
                m_Logger.LogError($"Container with key '{key}' not found.");
                return Optional<DataReference>.Fail();
            }
            
            var container = containerData.Data;
            HLogger.LogError($"container {containerData.GetContainerKey()} {container.Count}");
            for (int i = 0; i < container.Count; ++i)
            {
                if (container[i] == null)
                {
                    container[i] = data;
                    HLogger.LogError($"qwe {i}");
                    return Optional<DataReference>.Success(new DataReference(key, i));
                }
            }
            
            HLogger.LogError($"> data {data}");
            container.Add(data);
            HLogger.LogError($"< {container[0]}");
            var dataReference = new DataReference(key, container.Count - 1);
            HLogger.LogError($"asd {container.Count - 1}");
            return Optional<DataReference>.Success(dataReference);
        }
        
        public Optional<DataReference> RemoveData(string key, object data)
        {
            if (!TryGetContainer(key, out var containerData))
            {
                return Optional<DataReference>.Fail();
            }
            
            var items = containerData.Data;
            for (int i = 0; i < items.Count; ++i)
            {
                if (ReferenceEquals(items[i], data))
                {
                    items[i] = null;
                    var reference = new DataReference(key, i);
                    return Optional<DataReference>.Success(reference);
                }
            }

            return Optional<DataReference>.Fail();
        }

        public Optional<object> GetData(IDataReference dataReference)
        {
            if (!TryGetContainer(dataReference.Key, out var containerData))
            {
                HLogger.LogError("Container not found for key: " + dataReference.Key);
                return Optional<object>.Fail();
            }

            var data = containerData.Data;
            if (dataReference.Index < 0 || dataReference.Index >= data.Count)
            {
                HLogger.LogError($"Data reference index out of bounds: {dataReference.Index} for key: {dataReference.Key}");
                return Optional<object>.Fail();
            }
            
            return Optional<object>.Success(data[dataReference.Index]);
        }

        public Optional<T> GetData<T>(IDataReference dataReference) where T : class
        {
            var data = GetData(dataReference);
            if (!data.HasValue)
            {
                HLogger.LogError("Data not found for reference: " + dataReference);
                return Optional<T>.Fail();
            }

            if (data.Value == null)
            {
                HLogger.LogError($"Data is null for reference: {dataReference}");
                return Optional<T>.Fail();
            }
            
            if (data.Value is not T tObj)
            {
                HLogger.LogError($"Data type mismatch: expected {typeof(T)}, got {data.Value.GetType()} for reference: {dataReference}");
                return Optional<T>.Fail();
            }
            
            return Optional<T>.Success(tObj);
        }

        public Optional<IContainerData> GetContainer(string key)
        {
            if (!m_DataContainers.TryGetValue(key, out var containerData))
            {
                return Optional<IContainerData>.Fail();
            }
            
            return Optional<IContainerData>.Success(containerData);
        }

        public bool TryGetContainer(string key, out IContainerData container)
        {
            if (m_DataContainers.TryGetValue(key, out var containerData))
            {
                HLogger.LogError($"Get container {containerData.GetContainerKey()} {containerData.Data.Count}");
                container = containerData;
                return true;
            }

            container = null;
            return false;
        }
    }
}