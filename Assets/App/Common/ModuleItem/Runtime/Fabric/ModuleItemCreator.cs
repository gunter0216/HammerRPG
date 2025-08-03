using System;
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
        private readonly IModuleItemConfigController m_ConfigController;
        private readonly IContainersDataManager m_ContainerController;
        private readonly IReadOnlyList<ICreateModuleItemHandler> m_Handlers;

        public ModuleItemCreator(
            IModuleItemConfigController configController, 
            IContainersDataManager containerController, 
            IReadOnlyList<ICreateModuleItemHandler> handlers)
        {
            m_ConfigController = configController;
            m_ContainerController = containerController;
            m_Handlers = handlers;
        }

        public ModuleItemResult<IModuleItem> Create(string id)
        {
            var dataReferences = new List<DataReference>();
            var data = new ModuleItemData(id, dataReferences);
            
            var moduleItemResult = Create(data);
            if (!moduleItemResult.success)
            {
                return ModuleItemResult<IModuleItem>.Fail(moduleItemResult.errorMessage);
            }
            
            var dataReference = m_ContainerController.AddData(ModuleItemData.ContainerKey, data);
            if (!dataReference.HasValue)
            {
                return ModuleItemResult<IModuleItem>.Fail("Failed to add data reference.");
            }
            
            return ModuleItemResult<IModuleItem>.Success(moduleItemResult.moduleItem, dataReference.Value);
        }

        public ModuleItemResult<IModuleItem> Create(DataReference dataReference)
        {
            var data = m_ContainerController.GetData<ModuleItemData>(dataReference);
            if (!data.HasValue)
            {
                return ModuleItemResult<IModuleItem>.Fail("Data not found for the given reference.");
            }
            
            var moduleItemResult = Create(data.Value);
            if (!moduleItemResult.success)
            {
                return ModuleItemResult<IModuleItem>.Fail(moduleItemResult.errorMessage);
            }
            
            return ModuleItemResult<IModuleItem>.Success(moduleItemResult.moduleItem, dataReference);
        }

        private (IModuleItem moduleItem, string errorMessage, bool success) Create(IModuleItemData data)
        {
            var config = m_ConfigController.GetConfig(data.Id);
            if (!config.HasValue)
            {
                return (null, "Config not found", false);
            }

            var modulesHolder = new ModulesHolder(m_ContainerController, data.ModuleRefs);
            modulesHolder.Initialize();
            IModuleItem moduleItem = new ModuleItem(modulesHolder, config.Value, data);
            foreach (var handler in m_Handlers)
            {
                var handledGameItem = handler.Handle(moduleItem);
                if (!handledGameItem.HasValue)
                {
                    return (null, "Handler fuck up it.", false);
                }

                moduleItem = handledGameItem.Value;
            }
            
            return (moduleItem, "", true);
        }
    }
}