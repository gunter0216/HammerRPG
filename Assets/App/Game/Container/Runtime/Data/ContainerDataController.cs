using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Container.Runtime.Data.Loader;
using App.Game.Container.Runtime.Data.Model;

namespace App.Game.Container.Runtime.Data
{
    public class ContainerDataController : IContainerDataController
    {
        private readonly IDataManager m_DataManager;
        
        private ContainersData m_Data;
        
        public ContainerDataController(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public bool Initialize()
        {
            var dataLoader = new ContainerDataLoader(m_DataManager);
            var data = dataLoader.Load();
            if (!data.HasValue)
            {
                HLogger.LogError("ContainerData is null");
                return false;
            }
            
            m_Data = data.Value;
            
            m_Data.Containers ??= new List<ContainerData>();
            
            return true;
        }

        public Optional<ContainerData> GetContainer(int guid)
        {
            var containers = m_Data.Containers;
            if (guid < 0 || guid >= containers.Count)
            {
                HLogger.LogError($"Incorrect guid {guid}.");
                return Optional<ContainerData>.Fail();
            }

            var container = containers[guid];
            if (container == null)
            {
                HLogger.LogError($"Container not found {guid}");
                return Optional<ContainerData>.Fail();
            }
            
            return Optional<ContainerData>.Success(container);
        }

        public Optional<ContainerData> CreateContainer(int length)
        {
            var container = new ContainerData
            {
                Items = new List<ContainerItemData>(),
                Length = length
            };
            
            for (int i = 0; i < m_Data.Containers.Count; ++i)
            {
                if (m_Data.Containers[i] == null)
                {
                    m_Data.Containers[i] = container;
                    container.Guid = i;
                    
                    return Optional<ContainerData>.Success(container);
                }
            }

            container.Guid = m_Data.Containers.Count;
            m_Data.Containers.Add(container);
            
            return Optional<ContainerData>.Success(container);
        }

        public Optional<ContainerData> DestroyContainer(int guid)
        {
            var container = GetContainer(guid);
            if (!container.HasValue)
            {
                HLogger.LogError($"Container not found.");
                return Optional<ContainerData>.Fail();
            }

            m_Data.Containers[guid] = null;
            
            return Optional<ContainerData>.Success(container.Value);
        }

        public IReadOnlyList<ContainerData> GetContainers()
        {
            return m_Data.Containers.Where(x => x != null).ToArray();
        }
    }
}