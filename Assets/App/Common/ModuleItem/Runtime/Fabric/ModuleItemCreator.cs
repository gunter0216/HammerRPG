using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.ModuleItem.Runtime.Fabric.Interfaces;
using App.Common.ModuleItem.Runtime.Services;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Fabric
{
    public class ModuleItemCreator : IModuleItemCreator
    {
        private readonly IGameItemConfigController m_ConfigController;
        private readonly IContainersDataManager m_ContainerController;
        private readonly IReadOnlyList<ICreateModuleItemHandler> m_Handlers;

        public ModuleItemCreator(
            IGameItemConfigController configController, 
            IContainersDataManager containerController, 
            IReadOnlyList<ICreateModuleItemHandler> handlers)
        {
            m_ConfigController = configController;
            m_ContainerController = containerController;
            m_Handlers = handlers;
        }

        public Optional<IModuleItem> Create(string id)
        {
            var dataReferences = new List<DataReference>();
            var data = new ModuleItemData(id, dataReferences);
            return Create(data);
        }

        public Optional<IModuleItem> Create(IModuleItemData data)
        {
            var config = m_ConfigController.GetConfig(data.Id);
            if (!config.HasValue)
            {
                return Optional<IModuleItem>.Fail();
            }

            var modulesHolder = new ModulesHolder(m_ContainerController, data.ModuleRefs);
            IModuleItem moduleItem = new ModuleItem(modulesHolder, config.Value, data);
            foreach (var handler in m_Handlers)
            {
                var handledGameItem = handler.Handle(moduleItem);
                if (!handledGameItem.HasValue)
                {
                    return Optional<IModuleItem>.Fail();
                }

                moduleItem = handledGameItem.Value;
            }
            
            return Optional<IModuleItem>.Success(moduleItem);
        }
    }
}