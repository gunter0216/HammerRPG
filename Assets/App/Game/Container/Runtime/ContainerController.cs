using System.Collections.Generic;
using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Container.Runtime.Data;
using App.Game.Container.Runtime.Data.Model;

namespace App.Game.Container.Runtime
{
    public class ContainerController : IInitSystem, IContainerController
    {
        private readonly IDataManager m_DataManager;
        private readonly IModuleItemsManager m_ModuleItemsManager;
        
        private ContainerDataController m_DataController;

        private Dictionary<int, Container> m_Containers;

        public ContainerController(IDataManager dataManager, IModuleItemsManager moduleItemsManager)
        {
            m_DataManager = dataManager;
            m_ModuleItemsManager = moduleItemsManager;
        }

        public void Init()
        {
            m_DataController = new ContainerDataController(m_DataManager);
            m_DataController.Initialize();

            m_Containers = new Dictionary<int, Container>();
            foreach (var dataContainer in m_DataController.GetContainers())
            {
                var container = CreateContainer(dataContainer);
                if (!container.HasValue)
                {
                    HLogger.LogError("Cant create container");
                    continue;
                }
                
                m_Containers.Add(dataContainer.Guid, container.Value);
            }
        }

        private Optional<Container> CreateContainer(ContainerData data)
        {
            var items = new List<IModuleItem>(data.Items.Count);
            foreach (var item in data.Items)
            {
                if (item == null || item.DataReference == null)
                {
                    items.Add(null);
                    continue;
                }
                
                var moduleItem = m_ModuleItemsManager.Create(item.DataReference);
                if (!moduleItem.HasValue)
                {
                    HLogger.LogError($"Cant create item.");
                    continue;
                }
                    
                items.Add(moduleItem.Value);
            }
            
            var container = CreateContainerInternal(data, items);
            
            return Optional<Container>.Success(container.Value);
        }
        
        private Optional<Container> CreateContainerInternal(ContainerData data, List<IModuleItem> items)
        {
            var container = new Container(data, items);
            return Optional<Container>.Success(container);
        }

        public Optional<Container> CreateContainer(int length)
        {
            var dataContainer = m_DataController.CreateContainer(length);
            if (!dataContainer.HasValue)
            {
                HLogger.LogError($"Cant create data");
                return Optional<Container>.Fail();
            }

            var moduleItems = new List<IModuleItem>(length);
            for (int i = 0; i < length; ++i)
            {
                moduleItems.Add(null);
            }

            var container = CreateContainerInternal(dataContainer.Value, moduleItems);
            
            return Optional<Container>.Success(container.Value);
        }

        public Optional<Container> GetContainer(int guid)
        {
            if (!m_Containers.TryGetValue(guid, out var container))
            {
                HLogger.LogError("Container not found.");
                return Optional<Container>.Fail();
            }
            
            return Optional<Container>.Success(container);
        }

        public void DestroyContainer(int guid)
        {
            var container = GetContainer(guid);
            if (!container.HasValue)
            {
                HLogger.LogError("Container not found.");
                return;
            }

            DestroyContainer(container.Value);
        }

        public void DestroyContainer(Container container)
        {
            foreach (var item in container.Items)
            {
                if (item != null)
                {
                    m_ModuleItemsManager.Destroy(item);
                }
            }
            
            m_DataController.DestroyContainer(container.Guid);
            m_Containers.Remove(container.Guid);
        }
    }
}