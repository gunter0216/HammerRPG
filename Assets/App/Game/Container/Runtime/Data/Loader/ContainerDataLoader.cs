using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Container.Runtime.Data.Model;

namespace App.Game.Container.Runtime.Data.Loader
{
    public class ContainerDataLoader
    {
        private readonly IDataManager m_DataManager;

        public ContainerDataLoader(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public Optional<ContainersData> Load()
        {
            var data = m_DataManager.GetData<ContainersData>(nameof(ContainersData));
            if (!data.HasValue)
            {
                HLogger.LogError("ContainerData is null");
                return Optional<ContainersData>.Fail();
            }
            
            return data;
        }
    }
}