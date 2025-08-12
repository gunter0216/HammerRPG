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
            if (!m_DataContainers.TryGetValue(key, out var containerData))
            {
                m_Logger.LogError($"Container with key '{key}' not found.");
                return Optional<DataReference>.Fail();
            }
            
            var container = containerData.Data;
            for (int i = 0; i < container.Count; ++i)
            {
                if (container[i] == null)
                {
                    container[i] = data;
                    return Optional<DataReference>.Success(new DataReference(key, i));
                }
            }
            
            container.Add(data);
            var dataReference = new DataReference(key, container.Count - 1);
            return Optional<DataReference>.Success(dataReference);
        }
        
        public Optional<DataReference> RemoveData(string key, object data)
        {
            if (!m_DataContainers.TryGetValue(key, out var containerData))
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
            if (!m_DataContainers.TryGetValue(dataReference.Key, out var containerData))
            {
                return Optional<object>.Fail();
            }

            var data = containerData.Data;
            if (dataReference.Index < 0 || dataReference.Index >= data.Count)
            {
                return Optional<object>.Fail();
            }
            
            return Optional<object>.Success(data[dataReference.Index]);
        }

        public Optional<T> GetData<T>(IDataReference dataReference) where T : class
        {
            var data = GetData(dataReference);
            if (!data.HasValue)
            {
                return Optional<T>.Fail();
            }

            if (data.Value is T tObj)
            {
                return Optional<T>.Success(tObj);
            }
            
            return Optional<T>.Fail();
        }

        public Optional<IContainerData> GetContainer(string key)
        {
            if (!m_DataContainers.TryGetValue(key, out var containerData))
            {
                return Optional<IContainerData>.Fail();
            }
            
            return Optional<IContainerData>.Success(containerData);
        }
    }
}