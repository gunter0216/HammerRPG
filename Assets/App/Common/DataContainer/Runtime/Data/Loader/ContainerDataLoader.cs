using System.Collections.Generic;
using App.Common.Data.Runtime;
using App.Common.Utility.Runtime;

namespace App.Common.DataContainer.Runtime.Data.Loader
{
    public class ContainerDataLoader : IContainerDataLoader
    {
        private readonly IDataManager m_DataManager;
        private readonly IReadOnlyList<IContainerData> m_DataContainers;

        public ContainerDataLoader(IDataManager dataManager, IReadOnlyList<IContainerData> dataContainers)
        {
            m_DataManager = dataManager;
            m_DataContainers = dataContainers;
        }

        public Optional<IReadOnlyList<IContainerData>> Load()
        {
            var dataContainers = new List<IContainerData>(m_DataContainers.Count);
            for (int i = 0; i < m_DataContainers.Count; ++i)
            {
                var dataContainer = m_DataContainers[i];
                var data = Load(dataContainer);
                if (!data.HasValue)
                {
                    continue;
                }
                
                dataContainers.Add(data.Value as IContainerData);
            }
            
            return Optional<IReadOnlyList<IContainerData>>.Success(dataContainers);
        }

        private Optional<IData> Load(IContainerData data)
        { 
            return m_DataManager.GetData(data.Name());
        }
    }
}