using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Json.Runtime.Deserializer;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Fabric;
using App.Common.ModuleItem.Runtime.Fabric.Interfaces;
using App.Common.ModuleItem.Runtime.Services;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.External
{
    public class ModuleItemsManager : IInitSystem, IModuleItemsManager
    {
        private readonly IContainersDataManager _containersDataManager;
        private readonly IReadOnlyList<IModuleDtoToConfigConverter> _moduleDtoToConfigConverters;
        private readonly List<ICreateModuleItemHandler> _createHandlers;
        private readonly List<IDestroyModuleItemHandler> _destroyHandlers;
        private readonly IJsonDeserializer _jsonDeserializer;
        private readonly ILogger _logger;

        private ModuleItemsConfigController _configController;
        private ModuleItemCreator _moduleItemCreator;
        private ModuleItemDestroyer _moduleItemDestroyer;

        public ModuleItemsManager(
            IContainersDataManager containersDataManager,
            List<IModuleDtoToConfigConverter> moduleDtoToConfigConverters,
            IJsonDeserializer jsonDeserializer,
            ILogger logger)
        {
            _containersDataManager = containersDataManager;
            _moduleDtoToConfigConverters = moduleDtoToConfigConverters;
            _createHandlers = new List<ICreateModuleItemHandler>();
            _destroyHandlers = new List<IDestroyModuleItemHandler>();
            _jsonDeserializer = jsonDeserializer;
            _logger = logger;
        }

        public void Init()
        {
            InitConfigController();
            InitItemsFabric();
            _moduleItemDestroyer = new ModuleItemDestroyer(
                _destroyHandlers);
        }

        private void InitConfigController()
        {
            _configController = new ModuleItemsConfigController();
        }

        private void InitItemsFabric()
        {
            _moduleItemCreator = new ModuleItemCreator(
                _configController, 
                _containersDataManager, 
                _createHandlers);
        }

        public void AddHandler(IReadOnlyList<ICreateModuleItemHandler> handlers)
        {
            _createHandlers.AddRange(handlers);
        }
        
        public void AddHandler(IReadOnlyList<IDestroyModuleItemHandler> handlers)
        {
            _destroyHandlers.AddRange(handlers);
        }

        public bool RegisterItems(IModuleItemsConfigLoader moduleItemsConfigLoader, string type)
        {
            var dto = moduleItemsConfigLoader.Load();
            if (!dto.HasValue)
            {
                HLogger.LogError($"[BaseModuleItemsManager] In method Init, cant load file.");
                return false;
            }

            var dtoConverter = new ModuleItemsDtoToConfigConverter(
                _jsonDeserializer,
                _logger,
                _moduleDtoToConfigConverters);
            var config = dtoConverter.Convert(dto.Value, type);
            if (!config.HasValue)
            {
                HLogger.LogError($"[BaseModuleItemsManager] In method Init, cant convert dto to configs.");
                return false;
            }

            return RegisterItems(config.Value.Configs, type);
        }

        public bool RegisterItems(IReadOnlyList<IModuleItemConfig> configs, string type)
        {
            return _configController.RegisterItems(configs, type);
        }

        public Optional<IModuleItem> Create(DataReference dataReference)
        {
            return _moduleItemCreator.Create(dataReference);
        }

        public Optional<IModuleItem> Create(string id)
        {
            return _moduleItemCreator.Create(id);
        }

        public bool Destroy(IModuleItem item)
        {
            return _moduleItemDestroyer.Destroy(item);
        }

        public Optional<IModuleItemConfig> GetConfig(string id)
        {
            return _configController.GetConfig(id);
        }

        public Optional<IReadOnlyList<IModuleItemConfig>> GetConfigs(string type)
        {
            return _configController.GetConfigs(type);
        }
    }
}