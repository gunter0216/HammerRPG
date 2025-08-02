using System.Collections.Generic;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.DataContainer.Runtime.Data;
using App.Common.DataContainer.Runtime.Data.Loader;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Utility.Runtime;
using App.Game.States.Start;

namespace App.Common.DataContainer.External
{
    [Singleton]
    [Stage(typeof(StartInitPhase), 0)]
    public class ContainersDataManagerProxy : IInitSystem, IContainersDataManager
    {
        [Inject] private readonly IDataManager m_DataManager;
        [Inject] private readonly List<IContainerData> m_DataContainers;

        private ContainersDataManager m_ContainersDataManager;

        public void Init()
        {
            var logger = new Logger.Runtime.Logger();
            m_ContainersDataManager = new ContainersDataManager(
                new ContainerDataLoader(m_DataManager, m_DataContainers),
                logger);
            m_ContainersDataManager.Initialize();
        }

        public Optional<DataReference> AddData(string key, object data)
        {
            return m_ContainersDataManager.AddData(key, data);
        }

        public Optional<DataReference> RemoveData(string key, object data)
        {
            return m_ContainersDataManager.RemoveData(key, data);
        }

        public Optional<object> GetData(IDataReference dataReference)
        {
            return m_ContainersDataManager.GetData(dataReference);
        }

        public Optional<T> GetData<T>(IDataReference dataReference) where T : class
        {
            return m_ContainersDataManager.GetData<T>(dataReference);
        }
    }
}