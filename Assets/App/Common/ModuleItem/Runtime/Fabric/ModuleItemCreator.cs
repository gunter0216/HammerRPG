using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Data;
using App.Common.ModuleItem.Runtime.Fabric.Interfaces;
using App.Common.ModuleItem.Runtime.Services;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Fabric
{
    public class ModuleItemCreator : IModuleItemCreator
    {
        private readonly IModuleItemConfigController _configController;
        private readonly IContainersDataManager _containerController;
        private readonly IReadOnlyList<ICreateModuleItemHandler> _handlers;

        public ModuleItemCreator(
            IModuleItemConfigController configController,
            IContainersDataManager containerController,
            IReadOnlyList<ICreateModuleItemHandler> handlers)
        {
            _configController = configController;
            _containerController = containerController;
            _handlers = handlers;
        }

        public Optional<IModuleItem> Create(string id)
        {
            var dataReferences = new List<DataReference>();
            var data = new ModuleItemData(id.ToLower(), dataReferences);
            
            var dataReference = _containerController.AddData(ModuleItemData.ContainerKey, data);
            if (!dataReference.HasValue)
            {
                return Optional<IModuleItem>.Fail();
            }
            
            var moduleItemResult = Create(data, dataReference.Value);
            if (!moduleItemResult.HasValue)
            {
                _containerController.RemoveData(ModuleItemData.ContainerKey, data);
                HLogger.LogError("Failed to create module item for id: " + id);
                return Optional<IModuleItem>.Fail();
            }
            
            return Optional<IModuleItem>.Success(moduleItemResult.Value);
        }

        public Optional<IModuleItem> Create(DataReference dataReference)
        {
            var data = _containerController.GetData<ModuleItemData>(dataReference);
            if (!data.HasValue)
            {
                HLogger.LogError("Data not found for reference: " + dataReference);
                return Optional<IModuleItem>.Fail();
            }
            
            var moduleItemResult = Create(data.Value, dataReference);
            if (!moduleItemResult.HasValue)
            {
                HLogger.LogError("Failed to create module item for data: " + data.Value);
                return Optional<IModuleItem>.Fail();
            }
            
            return Optional<IModuleItem>.Success(moduleItemResult.Value);
        }

        private Optional<IModuleItem> Create(IModuleItemData data, DataReference referenceSelf)
        {
            var config = _configController.GetConfig(data.Id);
            if (!config.HasValue)
            {
                HLogger.LogError("Config not found for id: " + data.Id);
                return Optional<IModuleItem>.Fail();
            }

            var modulesHolder = new ModulesHolder(_containerController, data.ModuleRefs);
            modulesHolder.Initialize();
            IModuleItem moduleItem = new ModuleItem(modulesHolder, config.Value, data, referenceSelf);
            foreach (var handler in _handlers)
            {
                var handledGameItem = handler.OnItemCreated(moduleItem);
                if (!handledGameItem.HasValue)
                {
                    HLogger.LogError($"Handler {handler.GetType().Name} failed to handle item with id: {data.Id}");
                    return Optional<IModuleItem>.Fail();
                }

                moduleItem = handledGameItem.Value;
            }
            
            return Optional<IModuleItem>.Success(moduleItem);
        }
    }
}