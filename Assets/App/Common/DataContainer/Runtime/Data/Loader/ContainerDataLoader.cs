using System.Collections.Generic;
using App.Common.Data.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.DataContainer.Runtime.Data.Loader
{
    public class ContainerDataLoader : IContainerDataLoader
    {
        private readonly IDataManager m_DataManager;

        public ContainerDataLoader(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public Optional<IReadOnlyList<IContainerData>> Load()
        {
            var dataContainers = DataContainerRegistrar.GetDatas();
            var dataResult = new List<IContainerData>(dataContainers.Count);
            for (int i = 0; i < dataContainers.Count; ++i)
            {
                var dataContainer = dataContainers[i];
                var data = Load(dataContainer);
                if (!data.HasValue)
                {
                    continue;
                }
                
                dataResult.Add(data.Value as IContainerData);
            }
            
            return Optional<IReadOnlyList<IContainerData>>.Success(dataResult);
        }

        private Optional<IData> Load(IData data)
        { 
            return m_DataManager.GetData(data.Name());
        }
    }
}